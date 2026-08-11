using System;
using System.Collections.Generic;
using System.Text;
using UserManagement.Application.DTOs.Email;

namespace UserManagement.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailRequestDto request);
    }
}
