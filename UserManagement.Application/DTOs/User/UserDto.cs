using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Application.DTOs.User
{
    public class UserDto
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public List<string> Roles { get; set; }
    }
}
