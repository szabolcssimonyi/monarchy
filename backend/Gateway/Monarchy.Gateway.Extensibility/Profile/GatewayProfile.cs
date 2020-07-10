
using Monarchy.Authentication.Extensibility.Model;
using Monarchy.Gateway.Extensibility.Dto;
using Monarchy.Gateway.Extensibility.Model;

namespace Monarchy.Gateway.Extensibility.Profile
{
    public class GatewayProfile : AutoMapper.Profile
    {
        public GatewayProfile()
        {
            CreateMap<LoginDto, LoginModel>().ReverseMap();
            CreateMap<RoleDto, RoleModel>().ReverseMap();
            CreateMap<UserDto, UserModel>().ReverseMap();
            CreateMap<TokenDto, TokenModel>().ReverseMap();
        }
    }
}
