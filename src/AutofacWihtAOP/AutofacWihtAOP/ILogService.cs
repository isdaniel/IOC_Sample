namespace AutofacWithAOP
{
    public interface ILogService
    {
        LogModel GetLastLog(LogFilter filter);
        void AddLog(LogModel model);
    }
}