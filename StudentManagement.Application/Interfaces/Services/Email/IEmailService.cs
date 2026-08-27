namespace StudentManagement.Application.Interfaces.Services.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(
            string toEmail,
            string subject,
            string body);
    }
}