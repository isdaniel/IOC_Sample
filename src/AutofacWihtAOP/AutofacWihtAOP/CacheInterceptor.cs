using System;
using System.Runtime.Remoting.Messaging;
using Castle.DynamicProxy;

namespace AutofacWithAOP
{
    public class CacheInterceptor : IInterceptor
    {
        public string Data { get; set; }
        private ITimeService _timeService;
        public CacheInterceptor(ITimeService s)
        {
            
            _timeService = s;
        }

        public void Intercept(IInvocation invocation)
        {
            var key = invocation.GetConcreteMethod().Name;
            var time = CallContext.GetData(key)?.ToString();
            if (time == null)
            {
                Console.WriteLine("Set Cache Time");
                //如果沒有快取 執行呼叫Service
                invocation.Proceed();
                CallContext.SetData(key, invocation.ReturnValue);
            }
            else
            {
                //如果有快取直接取值
                Console.WriteLine("Get Time From Cache");
                invocation.ReturnValue = time;
            }
        }
    }
}