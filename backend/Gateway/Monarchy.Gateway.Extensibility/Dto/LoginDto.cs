using System.ComponentModel.DataAnnotations;

namespace Monarchy.Gateway.Extensibility.Dto
{
    public class LoginDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(32, MinimumLength = 8)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
