using System;

namespace AutofacWihtAOP
{
    [Function(Name = "Login")]
    public class UserModel
    {
        [Field(Name = "Phone")]
        public string Phone { get; set; }
        [Field(Name = "Birthday")]
        public DateTime Birthday { get; set; }
    }
}