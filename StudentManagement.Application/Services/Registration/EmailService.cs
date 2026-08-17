using System.Net;
using System.Net.Mail;
using StudentManagement.Application.Interfaces.Services;

namespace StudentManagement.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _senderEmail;
        private readonly string _senderPassword;

        public EmailService(
            string smtpServer,
            int smtpPort,
            string senderEmail,
            string senderPassword)
        {
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _senderEmail = senderEmail;
            _senderPassword = senderPassword;
        }

        public async Task SendRegistrationEmailAsync(
            string toEmail,
            string studentCode,
            string password,
            string studentName)
        {
            using var message = new MailMessage();

            message.From = new MailAddress(_senderEmail);
            message.To.Add(toEmail);

            message.Subject =
                "Student Registration Successful";

            message.Body = $@"
<html>
<body>
    <h2>Registration Successful</h2>

    <p>Dear {studentName},</p>

    <p>Your student registration has been completed successfully.</p>

    <p><strong>Your login details:</strong></p>

    <p>
        <strong>Student/User ID:</strong> {studentCode}<br/>
        <strong>Password:</strong> {password}
    </p>

    <p>Please keep these credentials safe.</p>

    <p>Thank you.</p>
</body>
</html>";

            message.IsBodyHtml = true;

            using var smtp = new SmtpClient(
                _smtpServer,
                _smtpPort);

            smtp.EnableSsl = true;

            smtp.Credentials = new NetworkCredential(
                _senderEmail,
                _senderPassword);

            await smtp.SendMailAsync(message);
        }
    }
}