using System.Collections.Generic;
using System.Linq;
using AutofacWihtAOP;

namespace AutofacWithAOP
{
    public class LogService : ILogService
    {
        List<AuditLogModel> _list = new List<AuditLogModel>();

        public AuditLogModel GetLastLog(LogFilter filter)
        {
            return _list.OrderBy(x => x.CreateDate)
                        .FirstOrDefault(filter.LogCondition);
        }

        public void AddLog(AuditLogModel model)
        {
            _list.Add(model);
        }
    }
}