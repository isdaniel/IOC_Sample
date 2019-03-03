using System.Collections.Generic;
using System.Linq;

namespace AutofacWihtAOP
{
    public class LogService : ILogService
    {
        List<LogModel> _list = new List<LogModel>();

        public LogModel GetLastLog(LogFilter filter)
        {
            return _list.OrderBy(x => x.CreateDate)
                .FirstOrDefault(x=>
                    x.FieldName == filter.FieldName && 
                    x.FunctionName == filter.FunctionName );
        }

        public void AddLog(LogModel model)
        {
            _list.Add(model);
        }
    }
}