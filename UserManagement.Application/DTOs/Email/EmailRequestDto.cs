using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Application.DTOs.Email
{
    public class EmailRequestDto
    {
        public string ToEmail { get; set; } = string.Empty;

        public string Subject { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;
    }
}
