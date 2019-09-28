using System;

namespace AutofacWithAOP
{
    public class TimeService : ITimeService
    {
        public string GetTime()
        {
            return DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss");
        }
    }
}