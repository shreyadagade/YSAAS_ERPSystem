using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StudentManagement.Application.Interfaces.Services.Login;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentManagement.Application.Services.Login
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(
            int studentId,
            string studentCode)
        {
            var jwtSettings =
                _configuration.GetSection("Jwt");

            var key =
                jwtSettings["Key"]
                ?? throw new Exception(
                    "JWT Key is not configured.");

            var issuer =
                jwtSettings["Issuer"]
                ?? throw new Exception(
                    "JWT Issuer is not configured.");

            var audience =
                jwtSettings["Audience"]
                ?? throw new Exception(
                    "JWT Audience is not configured.");

            var expiryMinutes =
                Convert.ToDouble(
                    jwtSettings["ExpiryMinutes"] ?? "60");

            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    studentId.ToString()),

                new Claim(
                    "StudentId",
                    studentId.ToString()),

                new Claim(
                    "StudentCode",
                    studentCode)
            };

            var securityKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(key));

            var credentials =
                new SigningCredentials(
                    securityKey,
                    SecurityAlgorithms.HmacSha256);

            var token =
                new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(
                        expiryMinutes),
                    signingCredentials: credentials);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}