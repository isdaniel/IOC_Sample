using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Castle.DynamicProxy;

namespace AutofacWihtAOP
{
    public class LogInterceptor : IInterceptor
    {
        private ILogService _logService;

        public LogInterceptor(ILogService logservice)
        {
            _logService = logservice;
        }

        public void Intercept(IInvocation invocation)
        {
            var models = invocation.Arguments.Where(Ext.IsAttributeType<FunctionAttribute>);

            var selectMany = models.SelectMany(x=> x.GetType().GetPropertiesBy<FieldAttribute>(), 
                (model,prop)=> new
                {
                    CurrentValue = prop.GetValue(model),
                    FieldName = prop.GetAttributeValue((FieldAttribute z) => z.Name),
                    functionName = model.GetType()
                        .GetAttributeValue((FunctionAttribute attr) => attr.Name)
                });

            foreach (var prop in selectMany)
            {
                
                var lastLog = _logService.GetLastLog(new LogFilter()
                {
                    FieldName = prop.FieldName,
                    FunctionName = prop.functionName
                });

                var logModel = new LogModel()
                {
                    UserCode = "Dnaiel",
                    FunctionName = prop.functionName,
                    FieldName = prop.FieldName,
                    NewValue = prop.CurrentValue?.ToString()
                };

                if (lastLog != null)
                    logModel.OldValue = lastLog.NewValue;
                else
                    logModel.OldValue = string.Empty;

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