using System.ComponentModel.DataAnnotations;

namespace Wa7at_ElDr3yah_API.DTOs.Auth
{
    public class ForgotPasswordDto
    {
        [Required]
        public string Email { get; set; } = string.Empty;
    }
}