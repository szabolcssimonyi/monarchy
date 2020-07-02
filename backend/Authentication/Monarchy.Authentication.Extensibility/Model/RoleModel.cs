using System.Collections;
using System.Collections.Generic;

namespace Monarchy.Authentication.Extensibility.Model
{
    public class RoleModel
    {
        public string Name { get; set; }
        public IEnumerable<string> Permissions { get; set; }
    }
}
