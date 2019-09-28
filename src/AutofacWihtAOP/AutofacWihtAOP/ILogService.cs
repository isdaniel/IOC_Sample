namespace AutofacWithAOP
{
    public interface ILogService
    {
        string GetLastLog(LogFilter filter);
        void AddLog(string message);
    }
}