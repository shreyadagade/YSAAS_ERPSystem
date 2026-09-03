
namespace LeadManagement.Application.DTOs.Login
{
    public class LoginResponseDto
    {
        public string UserId { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public List<string> Roles { get; set; } = new();

        public string Token { get; set; } = string.Empty;
    }
}

