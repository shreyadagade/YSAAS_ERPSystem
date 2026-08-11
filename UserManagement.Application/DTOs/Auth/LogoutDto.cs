using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Application.DTOs.Auth
{
    public class LogoutDto
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
