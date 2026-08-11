using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Application.DTOs.User
{
    public class ChangeUserStatusDto
    {
        public string UserId { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }
}
