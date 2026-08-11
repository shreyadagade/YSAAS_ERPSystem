using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Application.DTOs.Account
{
    public class ResetPasswordDto
    {
        public string EmailAddress { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
