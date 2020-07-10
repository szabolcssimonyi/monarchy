using Monarchy.Authentication.Extensibility.Model;

namespace Monarchy.Authentication.Extensibility.Profile
{
    public class AuthenticationProfile: AutoMapper.Profile
    {
        public AuthenticationProfile()
        {
            CreateMap<UserModel, UserEntity>().ReverseMap();
        }
    }
}
