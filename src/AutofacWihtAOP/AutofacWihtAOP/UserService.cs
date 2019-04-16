using System;
using Autofac.Extras.DynamicProxy;

namespace AutofacWithAOP
{
    [Intercept(typeof(LogInterceptor))]
    public class UserService:IUserService
    {
        public void ModifyUserInfo(UserModel model)
        {
            Console.WriteLine("User was modified");
        }
    }
}