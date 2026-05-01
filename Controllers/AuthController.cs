using Microsoft.AspNetCore.Mvc;
using Wa7at_ElDr3yah_API.DTOs.Auth;
using Wa7at_ElDr3yah_API.Services.Interfaces;

namespace Wa7at_ElDr3yah_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Login using email and password.
        /// </summary>
        /// <remarks>
        /// Returns a JWT token if the email and password are valid.
        /// The token must be used in protected endpoints using Bearer authentication.
        /// </remarks>
        /// <param name="dto">Login credentials.</param>
        /// <returns>JWT token and user information.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            if (result == null)
                return Unauthorized("Invalid email or password");

            return Ok(result);
        }

        /// <summary>
        /// Send a password reset code to the user's email.
        /// </summary>
        /// <remarks>
        /// Business rules:
        /// - User must exist by email.
        /// - Reset code is sent to the user's email.
        /// - Reset code is stored as a hash.
        /// - Reset code expires after a limited time.
        /// - User cannot request another reset code before the rate limit period ends.
        /// </remarks>
        /// <param name="dto">User email.</param>
        /// <returns>Confirmation message if the reset code was sent.</returns>
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            try
            {
                var result = await _authService.ForgotPasswordAsync(dto);

                if (!result)
                    return NotFound("User not found");

                return Ok(new
                {
                    message = "Reset code sent to your email"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        /// <summary>
        /// Reset user password using the verification code.
        /// </summary>
        /// <remarks>
        /// Business rules:
        /// - Reset code must be valid.
        /// - Reset code must not be expired.
        /// - New password must be strong.
        /// - New password is stored as a BCrypt hash.
        /// </remarks>
        /// <param name="dto">Email, reset code, and new password.</param>
        /// <returns>Confirmation message after password reset.</returns>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            try
            {
                var result = await _authService.ResetPasswordAsync(dto);

                if (!result)
                    return NotFound("User not found");

                return Ok(new
                {
                    message = "Password reset successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }
    }
}