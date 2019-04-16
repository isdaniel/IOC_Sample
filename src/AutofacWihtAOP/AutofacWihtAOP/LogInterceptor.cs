using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using AutofacWihtAOP;
using Castle.DynamicProxy;
using Xuenn.Lib.CommonUtility.Interceptors;

namespace AutofacWithAOP
{

    public class LogInterceptor : InterceptorBase
    {
        private ILogService _logService;

        public LogInterceptor(ILogService logservice)
        {
            _logService = logservice;
        }

        protected override void OnExcuted(IInvocation invocation)
        {
            var models = invocation.Arguments.Where(Ext.IsAttributeType<FunctionAttribute>);

            var props = models.SelectMany(x => x.GetType().GetPropertiesBy<FieldAttribute>(),
                (model, prop) => new
                {
                    CurrentValue = prop.GetValue(model),
                    FieldName = prop
                        .GetAttributeValue((FieldAttribute attr) => attr.Name),
                    functionName = model.GetType()
                        .GetAttributeValue((FunctionAttribute attr) => attr.FunctionName)
                });

            foreach (var prop in props)
            {

                AuditLogModel lastLog = _logService.GetLastLog(new LogFilter()
                {
                    FieldName = prop.FieldName,
                    FunctionName = prop.functionName,
                    UserCode = "Dnaiel"
                });

                AuditLogModel logModel = new AuditLogModel()
                {
                    UserCode = "Dnaiel",
                    FunctionName = prop.functionName,
                    FieldName = prop.FieldName,
                    NewValue = prop.CurrentValue?.ToString(),
                    OldValue = lastLog != null ? lastLog.NewValue :string.Empty
                };

                if (logModel?.NewValue != logModel.OldValue)
                    _logService.AddLog(logModel);
              
            }
        }
    }

    public static class Ext
    {
        public static IEnumerable<PropertyInfo> GetPropertiesBy<T>
            (this Type type,bool inherit = true) 
            where T : Attribute
        {
            if (type == null)
                throw new Exception("type can't be null");
            

            return type.GetProperties()
                       .Where(x => x.GetCustomAttribute<T>(inherit) != null);
        }

        public static bool IsAttributeType<TAttr>(this object obj)
            where TAttr : Attribute
        {
            return IsAttributeType<TAttr>(obj, true);
        }

        public static bool IsAttributeType<TAttr>(this object obj,bool inherit)
            where TAttr : Attribute
        {
            return obj.GetType()
                       .GetCustomAttribute<TAttr>(inherit) != null;
        }

        public static TRtn GetAttributeValue<TAttr, TRtn>(this PropertyInfo prop,
            Func<TAttr,TRtn> selector)
            where  TAttr : Attribute
        {
            TRtn result = default(TRtn);

            var attr = prop.GetCustomAttribute<TAttr>();

            if (selector == null || attr == null)
                return result;

            return selector(attr);
        }

        public static TRtn GetAttributeValue<TAttr, TRtn>(this Type prop,
            Func<TAttr, TRtn> selector)
            where TAttr : Attribute
        {
            TRtn result = default(TRtn);

            var attr = prop.GetCustomAttribute<TAttr>();

            if (selector == null || attr == null)
                return result;

            return selector(attr);
        }
    }
}