using System;
using Autofac.Extras.DynamicProxy;

namespace AutofacWithAOP
{
    [Intercept(typeof(CacheInterceptor))]
    [Intercept(typeof(LogInterceptor))]
    public class Person : IPerson
    {
        public string SaySomething()
        {
            return DateTime.Now.ToLongTimeString();
        }
    }
}