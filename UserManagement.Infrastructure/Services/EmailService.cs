using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using UserManagement.Application.Configuration;
using UserManagement.Application.DTOs.Email;
using UserManagement.Application.Interfaces;

namespace UserManagement.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(EmailRequestDto request)
        {
            using var message = new MailMessage();

            message.From = new MailAddress(_emailSettings.Email);

            message.To.Add(request.ToEmail);

            message.Subject = request.Subject;

            message.Body = request.Body;

            message.IsBodyHtml = true;

            using var smtpClient = new SmtpClient(_emailSettings.Host,_emailSettings.Port);

            smtpClient.EnableSsl = true;

            smtpClient.Credentials = new NetworkCredential( _emailSettings.Email,_emailSettings.Password);

            await smtpClient.SendMailAsync(message);
        }
    }
}
