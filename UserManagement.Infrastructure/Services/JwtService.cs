using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UserManagement.Application.Configuration;
using UserManagement.Application.Interfaces;
using UserManagement.Infrastructure.Persistence.Identity;

namespace UserManagement.Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<ApplicationUser> _userManager;

        public JwtService(
            IOptions<JwtSettings> jwtSettings,
            UserManager<ApplicationUser> userManager)
        {
            _jwtSettings = jwtSettings.Value;
            _userManager = userManager;
        }

        public async Task<string> GenerateTokenAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException(
                    "User ID is required.");
            }

            var user =
                await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new InvalidOperationException(
                    "User not found.");
            }

            if (!user.IsActive)
            {
                throw new InvalidOperationException(
                    "User account is deactivated.");
            }

            var roles =
                await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(
                    JwtRegisteredClaimNames.Sub,
                    user.Id),

                new Claim(
                    JwtRegisteredClaimNames.Email,
                    user.Email ?? string.Empty),

                new Claim(
                    ClaimTypes.Name,
                    user.UserName ?? string.Empty)
            };

            foreach (var role in roles)
            {
                claims.Add(
                    new Claim(
                        ClaimTypes.Role,
                        role));
            }

            var key =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        _jwtSettings.Key));

            var credentials =
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256);

            var token =
                new JwtSecurityToken(
                    issuer: _jwtSettings.Issuer,
                    audience: _jwtSettings.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(
                        _jwtSettings.ExpiryMinutes),
                    signingCredentials: credentials);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }

        public Task<string> GenerateRefreshTokenAsync()
        {
            var randomBytes = new byte[64];

            using var rng =
                RandomNumberGenerator.Create();

            rng.GetBytes(randomBytes);

            return Task.FromResult(
                Convert.ToBase64String(randomBytes));
        }
    }
}