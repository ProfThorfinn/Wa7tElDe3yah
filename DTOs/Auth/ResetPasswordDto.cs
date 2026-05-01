using System.ComponentModel.DataAnnotations;

namespace Wa7at_ElDr3yah_API.DTOs.Auth
{
    public class ResetPasswordDto
    {
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Code { get; set; } = string.Empty;

        [Required]
        public string NewPassword { get; set; } = string.Empty;
    }
}