using Monarchy.Authentication.Extensibility.Model;
using System.Collections.Generic;
using CoreConfiguration = Monarchy.Core.Extensibility.Configuration;

namespace Monarchy.Authentication.Extensibility
{
    public class Configuration: CoreConfiguration
    {
        public string ConnectionString { get; set; }
        public string AdministratorPassword { get; set; }
        public IEnumerable<RoleModel> Roles { get; set; }
    }
}
