using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Application.DTOs.Auth
{
    public class LoginResponseDto
    {
        public string Message { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
