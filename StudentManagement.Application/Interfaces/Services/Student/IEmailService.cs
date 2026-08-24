namespace StudentManagement.Application.Interfaces.Services.Student
{
    public interface IEmailService
    {
        Task SendEmailAsync(
            string toEmail,
            string subject,
            string body);
    }
}