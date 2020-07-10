using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace Monarchy.Gateway.Api.Filters
{
    public class DefaultAuthorizationHandler : AuthorizationHandler<AuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationRequirement requirement)
        {
            var permissions = context.User.Claims.Where(c => c.Type == "permission").Select(c => c.Value);
            if (permissions.Any(p => p == requirement.Permission))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
            return Task.CompletedTask;
        }
    }
}
