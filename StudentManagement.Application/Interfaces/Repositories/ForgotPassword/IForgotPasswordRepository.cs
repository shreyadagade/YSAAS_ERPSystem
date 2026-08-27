namespace StudentManagement.Application.Interfaces.Repositories.ForgotPassword
{
    public interface IForgotPasswordRepository
    {
        Task<bool> StudentExistsByEmailAsync(
            string emailAddress);

        Task<bool> ResetPasswordAsync(
            string emailAddress,
            string passwordHash);
    }
}