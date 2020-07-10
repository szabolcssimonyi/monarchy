using Monarchy.Authentication.Extensibility.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Monarchy.Authentication.Extensibility.Interface
{
    public interface IUserService
    {
        Task<UserEntity> GetUserByEmailAsync(string email);
        Task<IEnumerable<RoleModel>> GetRolesByUserAsync(UserEntity user);
        Task<bool> ValidateUserAsync(string email, string password);
    }
}
