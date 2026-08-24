using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.Infrastructure.Email
    {
        public class EmailSettings
        {
            public string SenderEmail { get; set; } = string.Empty;

            public string AppPassword { get; set; } = string.Empty;

            public string SmtpServer { get; set; } = string.Empty;

            public int SmtpPort { get; set; }
        }
    }

