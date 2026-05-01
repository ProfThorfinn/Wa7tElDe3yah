using Wa7at_ElDr3yah_API.DTOs.Auth;

namespace Wa7at_ElDr3yah_API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> LoginAsync(LoginDto dto);

        Task<bool> ForgotPasswordAsync(ForgotPasswordDto dto);

        Task<bool> ResetPasswordAsync(ResetPasswordDto dto);
    }
}