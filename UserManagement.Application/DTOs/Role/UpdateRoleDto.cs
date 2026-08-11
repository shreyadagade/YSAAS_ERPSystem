using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Application.DTOs.Role
{
    public class UpdateRoleDto
    {
        public string RoleId { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
    }
}
