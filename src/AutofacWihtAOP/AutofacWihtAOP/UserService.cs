using Autofac.Extras.DynamicProxy;

namespace AutofacWihtAOP
{
    [Intercept(typeof(LogInterceptor))]
    public class UserService:IUserService
    {
        public void ModifyUserInfo(UserModel model)
        {
            
        }
    }
}