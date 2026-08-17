namespace StudentManagement.Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendRegistrationEmailAsync(
            string toEmail,
            string studentCode,
            string password,
            string studentName);
    }
}