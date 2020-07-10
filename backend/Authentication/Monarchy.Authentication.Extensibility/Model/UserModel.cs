using System.Collections.Generic;

namespace Monarchy.Authentication.Extensibility.Model
{
    public class UserModel
    {
        public string Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public IEnumerable<RoleModel> Roles { get; set; }
    }
}
