using System;
using System.Collections.Generic;
using System.Text;
using UserManagement.Application.DTOs.Role;

namespace UserManagement.Application.Interfaces
{
    public interface IUserRoleService
    {
        Task<string> AssignRoleAsync(AssignRoleDto dto);
        Task<string> RemoveRoleAsync(RemoveRoleDto dto);
        Task<List<string>> GetUserRolesAsync(GetUserRolesDto dto);
    }
}
