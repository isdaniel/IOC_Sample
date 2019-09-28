using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Castle.DynamicProxy;

namespace AutofacWithAOP
{
    public class LogInterceptor : IInterceptor
    {
        public string Data { get; set; }
        public string Data1 { get; set; }
        private ILogService _logService;

        public LogInterceptor(ILogService logService)
        {
            _logService = logService;
        }

        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine("LogInterceptor Start");
            invocation.Proceed();
            Console.WriteLine("LogInterceptor End");
        }
    }
}