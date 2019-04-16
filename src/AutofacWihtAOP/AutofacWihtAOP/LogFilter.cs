using AutofacWihtAOP;

namespace AutofacWithAOP
{
    public class LogFilter
    {
        public string FunctionName { get; set; }
        public string FieldName { get; set; }
        public string UserCode { get; set; }

        public bool LogCondition(AuditLogModel x)
        {
            return x.FieldName == FieldName &&
                   x.FunctionName == FunctionName &&
                   x.UserCode == UserCode;
        }
    }
}