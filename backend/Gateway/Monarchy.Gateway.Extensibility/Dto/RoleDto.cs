using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Monarchy.Gateway.Extensibility.Dto
{
    public class RoleDto
    {
        [Required]
        [StringLength(64, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        public IEnumerable<string> Permissions { get; set; }
    }
}
