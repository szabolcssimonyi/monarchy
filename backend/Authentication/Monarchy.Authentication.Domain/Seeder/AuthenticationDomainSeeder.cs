using MassTransit.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Monarchy.Authentication.Extensibility;
using Monarchy.Authentication.Extensibility.Model;
using Monarchy.Core.Extensibility.Interface;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Monarchy.Authentication.Domain.Seeder
{
    public class AuthenticationDomainSeeder : IDomainSeeder
    {
        private readonly UserManager<UserEntity> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger<AuthenticationDomainSeeder> logger;
        private readonly Configuration configuration;
        private string AdminRoleName = "administrator";
        private UserEntity AdminUserModel = new UserEntity
        {
            Email = "admin@localhost.com",
            UserName = "Administrator",
        };

        public AuthenticationDomainSeeder(
            UserManager<UserEntity> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<Configuration> options,
            ILogger<AuthenticationDomainSeeder> logger)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.logger = logger;
            this.configuration = options.Value;
        }

        public async Task SeedAsync<T>(T context) where T : DbContext
        {
            await SeedAdminRole();
            await SeedAdminPermissions();
            await SeedAdminUser();
        }
        private async Task SeedAdminRole()
        {
            if ((await roleManager.FindByNameAsync(AdminRoleName)) == null)
            {
                var result = await roleManager.CreateAsync(new IdentityRole(AdminRoleName));
                if (!result.Succeeded)
                {
                    logger.LogError("Unable to create admin role in db");
                    return;
                }
            }
        }

        private async Task SeedAdminPermissions()
        {
            var role = roleManager.Roles.SingleOrDefault(r => r.Name == AdminRoleName);
            if (role == null)
            {
                logger.LogError("Unable to find admin role while trying to add permissions during the seed");
                return;
            }

            var claims = await roleManager.GetClaimsAsync(role);
            var optionClaims = configuration.Roles.SingleOrDefault(r => r.Name == AdminRoleName)?.Permissions;
            if (optionClaims == null)
            {
                logger.LogDebug("Permissions array is missing from configuration while seeding, permision seed is skipped");
                return;
            }
            var additionals = optionClaims.Where(oc => !claims.Select(c => c.Value).Contains(oc));
            var deletables = claims.Where(c => !optionClaims.Contains(c.Value));

            foreach (var add in additionals)
            {
                await roleManager.AddClaimAsync(role, new Claim("permission", add));
            }
            foreach (var del in deletables)
            {
                await roleManager.RemoveClaimAsync(role, del);
            }
            logger.LogDebug("Number of additional permissions: {@AddCount}, number of deleted permissions: {@DelCount} while seeding",
                additionals.Count(), deletables.Count());
        }

        private async Task SeedAdminUser()
        {
            var user = await userManager.FindByEmailAsync(AdminUserModel.Email);
            if (user != null)
            {
                logger.LogDebug("Admin user already exists in db");
                return;
            }

            var result = await userManager.CreateAsync(AdminUserModel, configuration.AdministratorPassword);
            if (!result.Succeeded)
            {
                logger.LogError("Unable to save admin user to db");
                return;
            }

            user = await userManager.FindByEmailAsync(AdminUserModel.Email);
            result = await userManager.AddToRoleAsync(user, AdminRoleName);
            if (!result.Succeeded)
            {
                logger.LogError("Unable to save admin role to user");
            }
        }
    }
}
