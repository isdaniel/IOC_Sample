using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;

namespace AutofacWithAOP
{
    class Program
    {

        static void Main(string[] args)
        {

            var container = GetContainer();

            IPerson person = container.Resolve<IPerson>();

            Console.WriteLine(person.SaySomething());
            Thread.Sleep(5000);
            Console.WriteLine(person.SaySomething());

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

            //將Assembly所有實現IInterceptor註冊入IOC容器中
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AssignableTo(typeof(IInterceptor));

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AsImplementedInterfaces()
                .EnableInterfaceInterceptors();

            //builder.RegisterType<UserService>()
            //    .As<IUserService>()
            //    .InterceptedBy(builder, 
            //        new 
            //            InterceptionData() { InterceptionType = typeof(LogInterceptor) }, 
            //        new InterceptionData(){ InterceptionType = typeof(CacheInterceptor) });

            return builder.Build();
        }
    }

    public class InterceptionData
    {
        public Type InterceptionType { get; set; }
        public IEnumerable<Parameter> Parameters { get; set; } = new List<Parameter>();
    }

    public static class InterceptionExtensions
    {
        public static IRegistrationBuilder<TLimit, ConcreteReflectionActivatorData, TRegistrationStyle>
            InterceptedBy<TLimit, TRegistrationStyle>(
                this IRegistrationBuilder<TLimit, ConcreteReflectionActivatorData, TRegistrationStyle> registration,
                ContainerBuilder builder,
                params InterceptionData[] datas)
        {

            registration.EnableInterception();

            var interceptions = datas.Where(x => typeof(IInterceptor).IsAssignableFrom(x.InterceptionType));

            registration.InterceptedBy(interceptions.Select(x => x.InterceptionType.FullName).ToArray());

            foreach (var data in datas)
            {
                builder.RegisterType(data.InterceptionType)
                    .WithProperties(data.Parameters)
                    .Named<IInterceptor>(data.InterceptionType.FullName);
            }

            return registration;
        }

        private static void EnableInterception<TLimit, TRegistrationStyle>(this IRegistrationBuilder<TLimit, ConcreteReflectionActivatorData, TRegistrationStyle> registration)
        {
            //判斷註冊type是否是Interface
            if (registration.RegistrationData
                .Services
                .OfType<IServiceWithType>().Any(x => x.ServiceType.IsInterface))
                registration.EnableInterfaceInterceptors();
            else
                registration.EnableClassInterceptors();
        }
    }
}
