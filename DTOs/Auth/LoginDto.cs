using System.ComponentModel.DataAnnotations;

namespace Wa7at_ElDr3yah_API.DTOs.Auth
{
    public class LoginDto
    {
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}