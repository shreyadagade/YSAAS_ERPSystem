using StudentManagement.Application.DTOs.ForgotPassword;

namespace StudentManagement.Application.Interfaces.Services.ForgotPassword
{
    public interface IForgotPasswordService
    {
        Task<string> GenerateResetTokenAsync(
            ForgotPasswordRequestDto request);

        Task ResetPasswordAsync(
            ResetPasswordRequestDto request);
    }
}