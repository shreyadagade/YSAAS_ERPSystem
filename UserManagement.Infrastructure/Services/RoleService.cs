using Microsoft.AspNetCore.Identity;
using UserManagement.Application.DTOs.Role;
using UserManagement.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using UserManagement.Application.Exceptions;

namespace UserManagement.Infrastructure.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<string> CreateRoleAsync(CreateRoleDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.RoleName))
            {
                throw new BadRequestException("Role name is required.");
            }

            var roleName = dto.RoleName.Trim();

            var existingRole = await _roleManager.RoleExistsAsync(roleName);

            if (existingRole)
            {
                throw new BadRequestException("Role already exists.");
            }

            var role = new IdentityRole(roleName);

            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                var errors =
                    string.Join(
                        ", ",
                        result.Errors.Select(
                            e => e.Description));

                throw new BadRequestException($"Role creation failed. {errors}");
            }

            return "Role created successfully.";
        }

        public async Task<List<RoleListDto>> GetRolesAsync()
        {
            return await _roleManager.Roles
                .Select(role => new RoleListDto
                {
                    RoleId = role.Id,
                    RoleName = role.Name!
                })
                .ToListAsync();
        }

        public async Task<string> UpdateRoleAsync(string roleId,UpdateRoleDto dto)
        {
            if (string.IsNullOrWhiteSpace(roleId))
            {
                throw new BadRequestException("Role ID is required.");
            }

            if (dto == null)
            {
                throw new BadRequestException("Role data is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.RoleName))
            {
                throw new BadRequestException("Role name is required.");
            }

            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                throw new NotFoundException("Role not found.");
            }

            var roleName = dto.RoleName.Trim();

            var existingRole = await _roleManager.FindByNameAsync(roleName);

            if (existingRole != null && existingRole.Id != role.Id)
            {
                throw new BadRequestException("Role name already exists.");
            }

            role.Name = roleName;

            var result = await _roleManager.UpdateAsync(role);

            if (!result.Succeeded)
            {
                var errors =
                    string.Join(
                        ", ",
                        result.Errors.Select(
                            e => e.Description));

                throw new BadRequestException($"Role update failed. {errors}");
            }

            return "Role updated successfully.";
        }

        public async Task<string> DeleteRoleAsync(string roleId)
        {
            if (string.IsNullOrWhiteSpace(roleId))
            {
                throw new BadRequestException("Role ID is required.");
            }

            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                throw new NotFoundException("Role not found.");
            }

            var result = await _roleManager.DeleteAsync(role);

            if (!result.Succeeded)
            {
                var errors =
                    string.Join(
                        ", ",
                        result.Errors.Select(
                            e => e.Description));

                throw new BadRequestException($"Role deletion failed. {errors}");
            }

            return "Role deleted successfully.";
        }

    }
}