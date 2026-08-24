using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using StudentManagement.Application.Interfaces.Services.Student;

namespace StudentManagement.Infrastructure.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(
            IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(
            string toEmail,
            string subject,
            string body)
        {
            using var message = new MailMessage();

            message.From = new MailAddress(
                _emailSettings.SenderEmail);

            message.To.Add(toEmail);

            message.Subject = subject;

            message.Body = body;

            message.IsBodyHtml = false;

            using var smtpClient =
                new SmtpClient(
                    _emailSettings.SmtpServer,
                    _emailSettings.SmtpPort);

            smtpClient.EnableSsl = true;

            smtpClient.Credentials =
                new NetworkCredential(
                    _emailSettings.SenderEmail,
                    _emailSettings.AppPassword);

            await smtpClient.SendMailAsync(message);
        }
    }
}

