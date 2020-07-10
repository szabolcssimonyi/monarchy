using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Monarchy.Authentication.Extensibility.Model;
using Monarchy.Gateway.Extensibility.Dto;
using Monarchy.Gateway.Extensibility.Interface;
using System.Linq;
using System.Threading.Tasks;

namespace Monarchy.Gateway.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IRequestClient<LoginModel> loginClient;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;

        public UserController(IRequestClient<LoginModel> loginClient, ITokenService tokenService, IMapper mapper)
        {
            this.loginClient = loginClient;
            this.tokenService = tokenService;
            this.mapper = mapper;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync(LoginDto login)
        {
            var model = mapper.Map<LoginModel>(login);
            var user = (await loginClient.GetResponse<UserModel>(model))?.Message;
            if (user == null)
            {
                return Unauthorized();
            }

            var permissions = user.Roles.SelectMany(r => r.Permissions).Distinct();
            return Ok(mapper.Map<TokenDto>(tokenService.Create(user.Email, permissions)));
        }
    }
}
