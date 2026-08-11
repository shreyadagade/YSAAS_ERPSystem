
using UserManagement.Application.DTOs.Account;

namespace UserManagement.Application.Interfaces
{
    public interface IAccountService
    {
        Task<string> ChangePasswordAsync(ChangePasswordDto dto);

        Task<string> ForgotPasswordAsync(ForgotPasswordDto dto);

        Task<string> ResetPasswordAsync(ResetPasswordDto dto);
    }
}
