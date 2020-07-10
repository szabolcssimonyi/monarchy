using Microsoft.AspNetCore.Identity;
using Monarchy.Authentication.Extensibility.Interface;
using Monarchy.Authentication.Extensibility.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monarchy.Authentication.Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<UserEntity> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserService(
            UserManager<UserEntity> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<IEnumerable<RoleModel>> GetRolesByUserAsync(UserEntity user)
        {
            if (string.IsNullOrEmpty(user?.Email))
            {
                return null;
            }
            var result = new List<RoleModel>();
            var roles = await userManager.GetRolesAsync(user);
            roles.ToList().ForEach(role =>
            {
                var claims = roleManager.GetClaimsAsync(roleManager.Roles.Single(r => r.Name == role))
                    .GetAwaiter().GetResult();
                result.Add(new RoleModel
                {
                    Name = role,
                    Permissions = claims
                        .Where(c => c.Type == "permission")
                        .Select(c => c.Value)
                });
            });
            return result;
        }

        public async Task<UserEntity> GetUserByEmailAsync(string email)
        {
            return await userManager.FindByEmailAsync(email);
        }

        public async Task<bool> ValidateUserAsync(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return false;
            }
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }
            return await userManager.CheckPasswordAsync(user, password);
        }
    }
}
