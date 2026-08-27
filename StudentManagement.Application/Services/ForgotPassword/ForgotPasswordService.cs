using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StudentManagement.Application.DTOs.ForgotPassword;
using StudentManagement.Application.Interfaces.Repositories.ForgotPassword;
using StudentManagement.Application.Interfaces.Services.ForgotPassword;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentManagement.Application.Services.ForgotPassword
{
    public class ForgotPasswordService
        : IForgotPasswordService
    {
        private readonly IForgotPasswordRepository _repository;
        private readonly IConfiguration _configuration;

        public ForgotPasswordService(
            IForgotPasswordRepository repository,
            IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        // =====================================================
        // GENERATE RESET TOKEN
        // =====================================================

        public async Task<string> GenerateResetTokenAsync(
            ForgotPasswordRequestDto request)
        {
            if (request == null)
            {
                throw new ArgumentException(
                    "Forgot password data is required.");
            }

            if (string.IsNullOrWhiteSpace(
                request.EmailAddress))
            {
                throw new ArgumentException(
                    "Email address is required.");
            }

            string email =
                request.EmailAddress.Trim();

            // =================================================
            // CHECK STUDENT
            // =================================================

            var studentExists =
                await _repository
                    .StudentExistsByEmailAsync(email);

            if (!studentExists)
            {
                throw new ArgumentException(
                    "No student found with this email address.");
            }

            // =================================================
            // JWT SETTINGS
            // =================================================

            var jwtSettings =
                _configuration.GetSection("Jwt");

            var jwtKey =
                jwtSettings["Key"]
                ?? throw new Exception(
                    "JWT Key is not configured.");

            var jwtIssuer =
                jwtSettings["Issuer"]
                ?? throw new Exception(
                    "JWT Issuer is not configured.");

            var jwtAudience =
                jwtSettings["Audience"]
                ?? throw new Exception(
                    "JWT Audience is not configured.");

            // =================================================
            // CREATE RESET TOKEN
            // =================================================

            var claims =
                new[]
                {
                    new Claim(
                        ClaimTypes.Email,
                        email),

                    new Claim(
                        "Purpose",
                        "PasswordReset"),

                    new Claim(
                        JwtRegisteredClaimNames.Jti,
                        Guid.NewGuid().ToString())
                };

            var key =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtKey));

            var credentials =
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256);

            var token =
                new JwtSecurityToken(
                    issuer: jwtIssuer,
                    audience: jwtAudience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(10),
                    signingCredentials: credentials);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }

        // =====================================================
        // RESET PASSWORD
        // =====================================================

        public async Task ResetPasswordAsync(
            ResetPasswordRequestDto request)
        {
            if (request == null)
            {
                throw new ArgumentException(
                    "Reset password data is required.");
            }

            if (string.IsNullOrWhiteSpace(
                request.ResetToken))
            {
                throw new ArgumentException(
                    "Reset token is required.");
            }

            if (string.IsNullOrWhiteSpace(
                request.NewPassword))
            {
                throw new ArgumentException(
                    "New password is required.");
            }

            if (string.IsNullOrWhiteSpace(
                request.ConfirmPassword))
            {
                throw new ArgumentException(
                    "Confirm password is required.");
            }

            if (request.NewPassword !=
                request.ConfirmPassword)
            {
                throw new ArgumentException(
                    "New password and confirm password do not match.");
            }

            // =================================================
            // JWT SETTINGS
            // =================================================

            var jwtSettings =
                _configuration.GetSection("Jwt");

            var jwtKey =
                jwtSettings["Key"]
                ?? throw new Exception(
                    "JWT Key is not configured.");

            var jwtIssuer =
                jwtSettings["Issuer"]
                ?? throw new Exception(
                    "JWT Issuer is not configured.");

            var jwtAudience =
                jwtSettings["Audience"]
                ?? throw new Exception(
                    "JWT Audience is not configured.");

            // =================================================
            // VALIDATE TOKEN
            // =================================================

            var tokenHandler =
                new JwtSecurityTokenHandler();

            var key =
                Encoding.UTF8.GetBytes(jwtKey);

            ClaimsPrincipal principal;

            try
            {
                principal =
                    tokenHandler.ValidateToken(
                        request.ResetToken,
                        new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,

                            IssuerSigningKey =
                                new SymmetricSecurityKey(key),

                            ValidateIssuer = true,

                            ValidIssuer =
                                jwtIssuer,

                            ValidateAudience = true,

                            ValidAudience =
                                jwtAudience,

                            ValidateLifetime = true,

                            ClockSkew =
                                TimeSpan.Zero
                        },
                        out _);
            }
            catch
            {
                throw new UnauthorizedAccessException(
                    "Invalid or expired reset token.");
            }

            // =================================================
            // CHECK PURPOSE
            // =================================================

            var purpose =
                principal.FindFirst("Purpose")?.Value;

            if (purpose != "PasswordReset")
            {
                throw new UnauthorizedAccessException(
                    "Invalid password reset token.");
            }

            // =================================================
            // GET EMAIL FROM TOKEN
            // =================================================

            var email =
                principal.FindFirst(
                    ClaimTypes.Email)?.Value;

            if (string.IsNullOrWhiteSpace(email))
            {
                throw new UnauthorizedAccessException(
                    "Email address was not found in reset token.");
            }

            // =================================================
            // HASH PASSWORD
            // =================================================

            string passwordHash =
                BCrypt.Net.BCrypt.HashPassword(
                    request.NewPassword);

            // =================================================
            // UPDATE PASSWORD
            // =================================================

            var updated =
                await _repository.ResetPasswordAsync(
                    email,
                    passwordHash);

            if (!updated)
            {
                throw new ArgumentException(
                    "Password could not be reset.");
            }
        }
    }
}