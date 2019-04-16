using System;

namespace AutofacWithAOP
{
    public class FunctionAttribute : Attribute
    {
        public string FunctionName { get; set; }
        public string FunctionType { get; set; }
    }
}