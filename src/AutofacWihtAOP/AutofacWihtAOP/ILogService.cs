using AutofacWihtAOP;

namespace AutofacWithAOP
{
    public interface ILogService
    {
        AuditLogModel GetLastLog(LogFilter filter);
        void AddLog(AuditLogModel model);
    }
}