using Microsoft.AspNetCore.Authorization;

namespace Monarchy.Gateway.Api.Filters
{
    public class AuthorizationRequirement : IAuthorizationRequirement
    {
        public string Permission { get; set; }
        public AuthorizationRequirement(string permission)
        {
            Permission = permission;
        }
    }
}
