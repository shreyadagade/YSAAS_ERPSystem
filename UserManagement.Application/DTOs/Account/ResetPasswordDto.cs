using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Application.DTOs.Account
{
    public class ResetPasswordDto
    {
        public string EmailAddress { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;

        public string NewPassword { get; set; } = string.Empty;
    }
}
