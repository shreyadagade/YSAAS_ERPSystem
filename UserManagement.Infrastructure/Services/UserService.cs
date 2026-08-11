using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using UserManagement.Application.DTOs.User;
using UserManagement.Application.Interfaces;
using UserManagement.Infrastructure.Persistence.Identity;

namespace UserManagement.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> ChangeUserStatusAsync(ChangeUserStatusDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserId))
            {
                throw new ArgumentException(
                    "User ID is required.");
            }

            var user = await _userManager.FindByIdAsync(
                    dto.UserId);

            if (user == null)
            {
                throw new InvalidOperationException(
                    "User not found.");
            }

            user.IsActive = dto.IsActive;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errors =
                    string.Join(
                        ", ",
                        result.Errors.Select(
                            e => e.Description));

                throw new InvalidOperationException(
                    $"User status update failed. {errors}");
            }

            return dto.IsActive
                ? "User activated successfully."
                : "User deactivated successfully.";
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = _userManager.Users.ToList();

            var userList = new List<UserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                userList.Add(new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.UserName,
                    Roles = roles.ToList()
                });
            }

            return userList;
        }

        public async Task<UserDto> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            var roles = await _userManager.GetRolesAsync(user);

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.UserName,
                Roles = roles.ToList()
            };
        }

        public async Task<bool> UpdateUserAsync(UpdateUserDto model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.UserName = model.FullName;
            user.Email = model.Email;
            user.NormalizedEmail = model.Email.ToUpper();
            user.NormalizedUserName = model.FullName.ToUpper();

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new Exception("User update failed");
            }

            return true;
        }

        public async Task<bool> DeleteUserAsync(DeleteUserDto model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                throw new Exception("User delete failed");
            }

            return true;
        }

    }
}
