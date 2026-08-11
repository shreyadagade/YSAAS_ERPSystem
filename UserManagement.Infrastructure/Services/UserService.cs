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

        public async Task<bool> UpdateUserAsync(UpdateUserDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentException("Update data is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.UserId))
            {
                throw new ArgumentException("User ID is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.FullName))
            {
                throw new ArgumentException("Full name is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                throw new ArgumentException("Email address is required.");
            }

            var user =
                await _userManager.FindByIdAsync(dto.UserId);

            if (user == null)
            {
                throw new InvalidOperationException(
                    "User not found.");
            }

            var existingUser = await _userManager.FindByEmailAsync(dto.Email.Trim());

            if (existingUser != null &&
                existingUser.Id != user.Id)
            {
                throw new InvalidOperationException(
                    "Email address is already registered.");
            }

            user.UserName = dto.FullName.Trim();
            user.Email = dto.Email.Trim();

            var result =
                await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errors =
                    string.Join(
                        ", ",
                        result.Errors.Select(
                            e => e.Description));

                throw new InvalidOperationException(
                    $"User update failed. {errors}");
            }

            return true;
        }


        public async Task<bool> DeleteUserAsync(DeleteUserDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentException(
                    "Delete data is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.UserId))
            {
                throw new ArgumentException(
                    "User ID is required.");
            }

            var user =
                await _userManager.FindByIdAsync(dto.UserId);

            if (user == null)
            {
                throw new InvalidOperationException(
                    "User not found.");
            }

            var result =
                await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                var errors =
                    string.Join(
                        ", ",
                        result.Errors.Select(
                            e => e.Description));

                throw new InvalidOperationException(
                    $"User delete failed. {errors}");
            }

            return true;
        }

    }
}
