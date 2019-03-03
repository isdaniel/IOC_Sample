using System;

namespace AutofacWihtAOP
{
    public class FieldAttribute : Attribute
    {
        public string Name { get; set; }
    }
}