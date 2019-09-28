using System.Collections.Generic;
using System.Linq;


namespace AutofacWithAOP
{
    public class LogService : ILogService
    {
        public void AddLog(string message)
        {
            
        }

        string ILogService.GetLastLog(LogFilter filter)
        {
            return string.Empty;
        }
    }
}