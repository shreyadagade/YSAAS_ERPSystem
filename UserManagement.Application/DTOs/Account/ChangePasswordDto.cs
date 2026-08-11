using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Application.DTOs.Account
{
    public class ChangePasswordDto
    {
        public string UserId { get; set; } = string.Empty;

        public string CurrentPassword { get; set; } = string.Empty;

        public string NewPassword { get; set; } = string.Empty;
    }
}
