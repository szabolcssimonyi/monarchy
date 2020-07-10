using System.ComponentModel.DataAnnotations;

namespace Monarchy.Gateway.Extensibility.Dto
{
    public class TokenDto
    {
        [Required]
        [StringLength(32)]
        public string AccessToken { get; set; }
    }
}
