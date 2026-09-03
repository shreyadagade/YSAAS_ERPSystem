
using LeadManagement.Application.DTOs.Login;
using LeadManagement.Application.Interfaces.Repositories.Login;
using LeadManagement.Application.Interfaces.Services;
using LeadManagement.Application.Interfaces.Services.Login;

namespace LeadManagement.Application.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IJwtService _jwtService;

        public LoginService(
            ILoginRepository loginRepository,
            IJwtService jwtService)
        {
            _loginRepository = loginRepository;
            _jwtService = jwtService;
        }

        public async Task<LoginResponseDto?> LoginAsync(
            LoginRequestDto request)
        {
            var result =
                await _loginRepository.ValidateLoginAsync(
                    request.UserName,
                    request.Password);

            if (!result.Success)
            {
                return null;
            }

            // Convert comma-separated roles into List<string>
            var roles = result.Role
                .Split(
                    ',',
                    StringSplitOptions.RemoveEmptyEntries |
                    StringSplitOptions.TrimEntries)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            // Generate JWT with all roles
            var token = _jwtService.GenerateToken(
                result.UserId,
                result.UserName,
                result.Role);

            return new LoginResponseDto
            {
                UserId = result.UserId,
                UserName = result.UserName,
                Roles = roles,
                Token = token
            };
        }
    }
}

