using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using Wa7at_ElDr3yah_API.Data;
using Wa7at_ElDr3yah_API.DTOs.Auth;
using Wa7at_ElDr3yah_API.Services.Interfaces;

namespace Wa7at_ElDr3yah_API.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null || !user.IsActive)
                return null;

            var isValidPassword = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

            if (!isValidPassword)
                return null;

            var duration = int.Parse(_configuration["Jwt:DurationInMinutes"]!);
            var expiresAt = DateTime.UtcNow.AddMinutes(duration);

            var claims = new List<Claim>
            {
                new Claim("uid", user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
            );

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials
            );

            return new AuthResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresAt = expiresAt,
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role.ToString()
            };
        }

        public async Task<bool> ForgotPasswordAsync(ForgotPasswordDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
                return false;

            if (user.LastResetPasswordRequestAt.HasValue &&
                user.LastResetPasswordRequestAt.Value.AddMinutes(2) > DateTime.UtcNow)
            {
                throw new Exception("Please wait 2 minutes before requesting another reset code");
            }

            var code = Random.Shared.Next(100000, 999999).ToString();

            user.ResetPasswordCodeHash = BCrypt.Net.BCrypt.HashPassword(code);
            user.ResetPasswordCodeExpiresAt = DateTime.UtcNow.AddMinutes(10);
            user.LastResetPasswordRequestAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var subject = "Reset Your Password - Wa7at ElDr3yah";

            var body = $@"
    <div style='font-family:Arial,sans-serif;background:#f5f5f5;padding:30px;'>
        <div style='max-width:600px;margin:auto;background:white;padding:30px;border-radius:10px;'>
            <h2 style='color:#2c3e50;'>Password Reset Request</h2>
            <p>Hello <b>{user.FullName}</b>,</p>
            <p>You requested to reset your password.</p>
            <p>Your verification code is:</p>
            <div style='font-size:32px;font-weight:bold;letter-spacing:6px;background:#eee;padding:15px;text-align:center;border-radius:8px;'>
                {code}
            </div>
            <p style='margin-top:20px;'>This code will expire in <b>10 minutes</b>.</p>
            <p>If you did not request this, please ignore this email.</p>
            <hr />
            <p style='color:#777;font-size:13px;'>Wa7at ElDr3yah Booking System</p>
        </div>
    </div>";

            await SendEmailAsync(user.Email, subject, body);

            return true;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
                return false;

            if (string.IsNullOrWhiteSpace(user.ResetPasswordCodeHash))
                throw new Exception("No reset code found");

            if (user.ResetPasswordCodeExpiresAt == null ||
                user.ResetPasswordCodeExpiresAt < DateTime.UtcNow)
                throw new Exception("Reset code has expired");

            var isValidCode = BCrypt.Net.BCrypt.Verify(dto.Code, user.ResetPasswordCodeHash);

            if (!isValidCode)
                throw new Exception("Invalid reset code");

            if (!IsStrongPassword(dto.NewPassword))
                throw new Exception("Password must be at least 8 characters and contain uppercase, lowercase, number and special character");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            user.ResetPasswordCodeHash = null;
            user.ResetPasswordCodeExpiresAt = null;
            user.LastResetPasswordRequestAt = null;

            await _context.SaveChangesAsync();

            return true;
        }

        private async Task SendEmailAsync(string to, string subject, string body)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");

            using var smtpClient = new SmtpClient(emailSettings["SmtpServer"])
            {
                Port = int.Parse(emailSettings["Port"]!),
                Credentials = new NetworkCredential(
                    emailSettings["Username"],
                    emailSettings["Password"]
                ),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(emailSettings["From"]!),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(to);

            await smtpClient.SendMailAsync(mailMessage);
        }

        private static bool IsStrongPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                return false;

            bool hasUpper = password.Any(char.IsUpper);
            bool hasLower = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSymbol = password.Any(c => !char.IsLetterOrDigit(c));

            return hasUpper && hasLower && hasDigit && hasSymbol;
        }
    }
}