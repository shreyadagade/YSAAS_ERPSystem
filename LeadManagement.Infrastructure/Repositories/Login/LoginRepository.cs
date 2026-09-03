
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using LeadManagement.Application.Interfaces.Repositories.Login;

namespace LeadManagement.Infrastructure.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        private readonly IConfiguration _configuration;

        public LoginRepository(
            IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<(bool Success, string UserId, string UserName, string Role)>
            ValidateLoginAsync(
                string userName,
                string password)
        {
            var connectionString =
                _configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "DefaultConnection is not configured.");
            }

            using var connection =
                new SqlConnection(connectionString);

            const string sql = @"
                SELECT
                    u.Id AS UserId,
                    u.UserName,
                    u.PasswordHash,
                    r.Name AS Role
                FROM erpsystem.AspNetUsers u
                INNER JOIN erpsystem.AspNetUserRoles ur
                    ON u.Id = ur.UserId
                INNER JOIN erpsystem.AspNetRoles r
                    ON ur.RoleId = r.Id
                WHERE u.UserName = @UserName
                  AND u.IsActive = 1;";

            var users = await connection.QueryAsync<LoginUserData>(
                sql,
                new
                {
                    UserName = userName
                });

            var userList = users.ToList();

            // No user found
            var user = userList.FirstOrDefault();

            if (user == null ||
                string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                return (
                    false,
                    string.Empty,
                    string.Empty,
                    string.Empty);
            }

            // Verify password
            var passwordHasher =
                new PasswordHasher<LoginUserData>();

            var passwordResult =
                passwordHasher.VerifyHashedPassword(
                    user,
                    user.PasswordHash,
                    password);

            if (passwordResult ==
                PasswordVerificationResult.Failed)
            {
                return (
                    false,
                    string.Empty,
                    string.Empty,
                    string.Empty);
            }

            // Get ALL roles assigned to the user
            var roles = userList
                .Where(x => !string.IsNullOrWhiteSpace(x.Role))
                .Select(x => x.Role!.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            // Convert all roles into one string.
            // Example:
            // Student,CTO,Counsellor,Trainer,Developer
            var allRoles = string.Join(",", roles);

            return (
                true,
                user.UserId,
                user.UserName ?? string.Empty,
                allRoles);
        }

        private class LoginUserData
        {
            public string UserId { get; set; } = string.Empty;

            public string? UserName { get; set; }

            public string? PasswordHash { get; set; }

            public string? Role { get; set; }
        }
    }
}

