using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using Monarchy.Authentication.Extensibility.Interface;
using Monarchy.Authentication.Extensibility.Model;
using System;
using System.Threading.Tasks;

namespace Monarchy.Authentication.Api.Consumer
{
    public class LoginConsumer : IConsumer<LoginModel>
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly ILogger<LoginConsumer> logger;

        public LoginConsumer(IUserService userService, IMapper mapper, ILogger<LoginConsumer> logger)
        {
            this.userService = userService;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task Consume(ConsumeContext<LoginModel> context)
        {
            logger.LogInformation("User login model arrived {@LoginModel}", context.Message);
            var user = await userService.GetUserByEmailAsync(context.Message.Email);
            if (user == null)
            {
                logger.LogWarning("User not found during login {@LoginModel}", context.Message);
                await context.RespondAsync<UserModel>(null);
                return;
            }
            var roles = await userService.GetRolesByUserAsync(user);
            var userModel = mapper.Map<UserModel>(user);
            userModel.Roles = roles;
            logger.LogInformation("User found during login {@UserModel}", userModel);
            await context.RespondAsync<UserModel>(userModel);
            return;
        }
    }
}
