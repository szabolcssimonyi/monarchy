using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Monarchy.Gateway.Extensibility.Dto
{
    public class UserDto
    {
        [Required]
        [StringLength(32)]
        public string Id { get; set; }

        [Required]
        [StringLength(64, MinimumLength = 2)]
        public string Firstname { get; set; }

        [Required]
        [StringLength(64, MinimumLength = 2)]
        public string Lastname { get; set; }

        [Required]
        [StringLength(64, MinimumLength = 8)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public IEnumerable<RoleDto> Roles { get; set; }
    }
}
