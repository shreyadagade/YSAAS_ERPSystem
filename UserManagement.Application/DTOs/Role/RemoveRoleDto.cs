using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Application.DTOs.Role
{
    public class RemoveRoleDto
    {
        public string UserId { get; set; } = string.Empty;
        public string RoleId { get; set; } = string.Empty;
    }
}
