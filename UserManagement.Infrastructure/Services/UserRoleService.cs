using Microsoft.AspNetCore.Identity;
using UserManagement.Application.DTOs.Role;
using UserManagement.Application.Exceptions;
using UserManagement.Application.Interfaces;
using UserManagement.Infrastructure.Persistence.Identity;

namespace UserManagement.Infrastructure.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRoleService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public async Task<string> AssignRoleAsync(AssignRoleDto dto)
        {
            if (dto == null)
            {
                throw new BadRequestException("Role assignment data is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.UserId))
            {
                throw new BadRequestException("User ID is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.RoleId))
            {
                throw new BadRequestException("Role ID is required.");
            }

            var user = await _userManager.FindByIdAsync(dto.UserId);

            if (user == null)
            {
                throw new NotFoundException(
                    "User not found.");
            }

            if (!user.IsActive)
            {
                throw new NotFoundException(
                    "User not found.");
            }

            var role = await _roleManager.FindByIdAsync(dto.RoleId);

            if (role == null)
            {
                throw new NotFoundException("Role not found.");
            }

            var alreadyAssigned = await _userManager.IsInRoleAsync(user,role.Name!);

            if (alreadyAssigned)
            {
                throw new InternalServerErrorException(
                    "Role is already assigned to the user.");
            }

            var result =
                await _userManager.AddToRoleAsync(
                    user,
                    role.Name!);

            if (!result.Succeeded)
            {
                var errors =
                    string.Join(
                        ", ",
                        result.Errors.Select(
                            e => e.Description));

                throw new BadRequestException($"Role assignment failed. {errors}");
            }

            return "Role assigned successfully.";
        }

        public async Task<string> RemoveRoleAsync(string userId,string roleId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new BadRequestException("User ID is required.");
            }

            if (string.IsNullOrWhiteSpace(roleId))
            {
                throw new BadRequestException("Role ID is required.");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            if (!user.IsActive)
            {
                throw new NotFoundException("User not found.");
            }

            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                throw new NotFoundException("Role not found.");
            }

            var isAssigned = await _userManager.IsInRoleAsync(user,role.Name!);

            if (!isAssigned)
            {
                throw new BadRequestException(
                    "Role is not assigned to the user.");
            }

            var result = await _userManager.RemoveFromRoleAsync(user,role.Name!);

            if (!result.Succeeded)
            {
                var errors =
                    string.Join(
                        ", ",
                        result.Errors.Select(
                            e => e.Description));

                throw new BadRequestException($"Role removal failed. {errors}");
            }

            return "Role removed successfully.";
        }

        public async Task<List<string>> GetUserRolesAsync(GetUserRolesDto dto)
        {
            if (dto == null)
            {
                throw new BadRequestException("User data is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.UserId))
            {
                throw new BadRequestException("User ID is required.");
            }

            var user = await _userManager.FindByIdAsync(dto.UserId);

            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            if (!user.IsActive)
            {
                throw new NotFoundException("User not found.");
            }


            var roles = await _userManager.GetRolesAsync(user);

            return roles.ToList();
        }
    }
}