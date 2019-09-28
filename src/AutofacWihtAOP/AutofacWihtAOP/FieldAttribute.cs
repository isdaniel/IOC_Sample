using System;

namespace AutofacWithAOP
{
    public class FieldAttribute : Attribute
    {
        public string Name { get; set; }
    }
}