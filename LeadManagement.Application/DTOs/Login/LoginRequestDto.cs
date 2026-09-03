using System;
using System.Collections.Generic;
using System.Text;

namespace LeadManagement.Application.DTOs.Login
{
    public class LoginRequestDto
    {
        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
