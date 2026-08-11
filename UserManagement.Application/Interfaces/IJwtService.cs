using System.Threading.Tasks;

namespace UserManagement.Application.Interfaces
{
    public interface IJwtService
    {
        Task<string> GenerateTokenAsync(string userId);

        Task<string> GenerateRefreshTokenAsync();
    }
}