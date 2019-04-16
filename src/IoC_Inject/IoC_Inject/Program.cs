using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace IoC_Inject
{
    public interface IWalkable
    {
        void Walk();

        void Walk(TextWriter writer);
    }

    public class Person : IWalkable
    {
        private readonly TextWriter _writer;

        public TextWriter WriterProp { get; set; }


        public Person(TextWriter w)
        {
            _writer = w;
        }

        public void Walk()
        {
            _writer.WriteLine("constructor walk");
            WriterProp.WriteLine("property walk");
        }

        public void Walk(TextWriter writer)
        {
            writer.WriteLine("walk by parameter");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            IContainer container = GetContainer();

            var w= container.Resolve<IWalkable>();
            w.Walk();

            Console.ReadKey();
        }

        private static IContainer GetContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.Register(x => Console.Out).As<TextWriter>();

            builder.RegisterType<Person>().As<IWalkable>();


            //註冊屬性和方法參數
            builder.RegisterType<Person>()
                   .As<IWalkable>().OnActivating(e =>
            {
               var writer = e.Context.Resolve<TextWriter>();
               e.Instance.WriterProp = writer;
               e.Instance.Walk(writer);
            });

            return builder.Build();
        }
    }
}
