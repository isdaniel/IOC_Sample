using System;

namespace AutofacWihtAOP
{
    public class LogModel
    {
        public DateTime CreateDate { get; set; } = DateTime.Now;

        public string OldValue { get; set; }

        public string NewValue { get; set; }

        public string UserCode { get; set; }

        public string FunctionName { get; set; }

        public string FieldName { get; set; }
    }
}