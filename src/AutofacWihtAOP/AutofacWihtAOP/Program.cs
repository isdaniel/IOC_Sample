using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Builder;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using static System.Convert;

namespace AutofacWihtAOP
{
    public interface ITimeService
    {
        string GetTime();
    }

    public class TimeService : ITimeService
    {
        public string GetTime()
        {
            return DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss");
        }
    }

    public class TimeInterceptor : IInterceptor
    {
        private ITimeService _timeService;
        public TimeInterceptor(ITimeService s)
        {
            
            _timeService = s;
        }

        public void Intercept(IInvocation invocation)
        {
            var time = CallContext.GetData("time")?.ToString();
            if (time == null)
            {
                //如果沒有快取 執行呼叫Service
                invocation.Proceed();
                CallContext.SetData("time", invocation.ReturnValue);
            }
            else
            {
                //如果有快取直接取值
                invocation.ReturnValue = time;
            }
        }
    }

    [Intercept(typeof(TimeInterceptor))]
    public class Person : IPerson
    {
        public string SaySomething()
        {
            return DateTime.Now.ToLongTimeString();
        }
    }

    public interface IPerson
    {
        string SaySomething();
    }


    class Program
    {

        static void Main(string[] args)
        {

            var container = GetContainer();

            IPerson person = container.Resolve<IPerson>();

            //Console.WriteLine(person.SaySomething());
            //Thread.Sleep(5000);
            //Console.WriteLine(person.SaySomething());

            IUserService personService = container.Resolve<IUserService>();

            personService.ModifyUserInfo(new UserModel()
            {
                Birthday = DateTime.Now,
                Phone = "0911181212"
            });

            Console.ReadKey();
        }

        private static IContainer GetContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<TimeInterceptor>(); //註冊攔截器
            builder.RegisterType<LogInterceptor>(); //註冊攔截器

            builder.RegisterType<LogService>()
                .As<ILogService>();

            builder.RegisterType<Person>()
                    .As<IPerson>()
                    .EnableInterfaceInterceptors();

            builder.RegisterType<UserService>()
                .As<IUserService>()
                .EnableInterfaceInterceptors();

            
            //註冊時間Service
            builder.RegisterType<TimeService>().As<ITimeService>();

            return builder.Build();
        }
    }
}
