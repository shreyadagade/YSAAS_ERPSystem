using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Application.DTOs.User
{
    public class UpdateUserDto
    {
        public string UserId { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }
    }
}
