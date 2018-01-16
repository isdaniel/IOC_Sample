using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IoC
{
    /// <summary>
    /// 小明直接依賴 Aunt 不是依賴抽象
    /// 日後要改必須動內部
    /// </summary>
    public class Mine
    {
        public Aunt aunt = new Aunt();

        public void Room()
        {
            aunt.Swapping();
        }
    }

    /// <summary>
    /// 小明
    /// </summary>
    public class MineWithMiddle
    {
        private ISwapable aunt;

        /// <summary>
        /// 依賴抽象[打掃動作的人]
        /// </summary>
        /// <param name="swapAble"></param>
        public MineWithMiddle(ISwapable swapAble)
        {
            aunt = swapAble;
        }

        public void Room()
        {
            aunt.Swapping();
        }
    }


    /// <summary>
    /// 依賴抽象[打掃動作的人]
    /// </summary>
    public class Aunt : ISwapable
    {
        public void Swapping()
        {
            Console.WriteLine("Aunt Swapping");
        }
    }

    /// <summary>
    /// 打掃動作
    /// </summary>
    public interface ISwapable
    {
        void Swapping();
    }

   

    class Program
    {
        static void Main(string[] args)
        {
            Mine mine = new Mine();
            mine.Room();

            IContainer middleCompany = MiddleCompany();
            //仲介公司(IOC AutoFac)自動幫小明注入一個打掃阿姨
            MineWithMiddle mineWithMiddle = middleCompany.Resolve<MineWithMiddle>();

            mineWithMiddle.Room();

            Console.ReadKey();
        }

        /// <summary>
        /// 仲介公司
        /// </summary>
        /// <returns></returns>
        private static IContainer MiddleCompany()
        {
            ContainerBuilder builder = new ContainerBuilder();

            //在仲介公司裡寫需求人申請單
            builder.RegisterType<MineWithMiddle>();
            //小明所需打掃阿姨需求
            builder.RegisterType<Aunt>().As<ISwapable>();

            return builder.Build();
        }
    }
}
