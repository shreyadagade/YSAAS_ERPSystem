using System;
using System.Collections.Generic;
using System.Text;
using UserManagement.Application.DTOs.Role;

namespace UserManagement.Application.Interfaces
{
    public interface IRoleService
    {
        Task<string> CreateRoleAsync(CreateRoleDto dto);
        Task<List<RoleListDto>> GetRolesAsync();
        Task<string> UpdateRoleAsync(UpdateRoleDto dto);
        Task<string> DeleteRoleAsync(DeleteRoleDto dto);
    }
}
