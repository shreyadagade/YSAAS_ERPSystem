using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using UserManagement.Application.DTOs.User;
using UserManagement.Application.Interfaces;
using UserManagement.Infrastructure.Persistence.Identity;
using UserManagement.Application.Contracts;
using Microsoft.EntityFrameworkCore;

namespace UserManagement.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGenericRepository _repository;

        private const string StoredProcedure = "erpsystem.sp_tblemployees";

        public UserService(UserManager<ApplicationUser> userManager,
            IGenericRepository repository)
        {
            _userManager = userManager;
            _repository = repository;
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

        public async Task<string> UpdateUserAsync(string userId,UpdateUserDto dto)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException(
                    "User ID is required.");
            }

            if (dto == null)
            {
                throw new ArgumentException(
                    "Update data is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.EmployeeName))
            {
                throw new ArgumentException(
                    "Employee name is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.EmailAddress))
            {
                throw new ArgumentException(
                    "Email address is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.MobileNumber))
            {
                throw new ArgumentException(
                    "Mobile number is required.");
            }

            var user =
                await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new InvalidOperationException(
                    "User not found.");
            }

            var existingUser =
                await _userManager.FindByEmailAsync(
                    dto.EmailAddress.Trim());

            if (existingUser != null &&
                existingUser.Id != user.Id)
            {
                throw new InvalidOperationException(
                    "Email address is already registered.");
            }

            var existingMobile = await _userManager.Users.FirstOrDefaultAsync(u =>
                u.PhoneNumber == dto.MobileNumber &&
                u.Id != user.Id);

            if (existingMobile != null)
            {
                throw new InvalidOperationException(
                    "A user with this mobile number already exists.");
            }
            user.Email = dto.EmailAddress.Trim();
            user.NormalizedEmail =
                dto.EmailAddress.Trim().ToUpper();

            user.PhoneNumber =
                dto.MobileNumber.Trim();

            var identityResult =
                await _userManager.UpdateAsync(user);

            if (!identityResult.Succeeded)
            {
                var errors =
                    string.Join(
                        ", ",
                        identityResult.Errors.Select(
                            e => e.Description));

                throw new InvalidOperationException(
                    $"User update failed. {errors}");
            }

            await _repository.ExecuteNonQueryAsync(StoredProcedure,

                new StoredProcedureParameter
                {
                    Name = "@Type",
                    Value = "UpdateUser"
                },

                new StoredProcedureParameter
                {
                    Name = "@user_id",
                    Value = userId
                },

                new StoredProcedureParameter
                {
                    Name = "@employee_name",
                    Value = dto.EmployeeName.Trim()
                },

                new StoredProcedureParameter
                {
                    Name = "@email_address",
                    Value = dto.EmailAddress.Trim()
                },

                new StoredProcedureParameter
                {
                    Name = "@mobile_number",
                    Value = dto.MobileNumber.Trim()
                });

            return "User profile updated successfully.";
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException(
                    "User ID is required.");
            }

            var user =
                await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new InvalidOperationException(
                    "User not found.");
            }

            var employeeResult =
                await _repository.ExecuteNonQueryAsync(
                    StoredProcedure,

                    new StoredProcedureParameter
                    {
                        Name = "@Type",
                        Value = "Delete"
                    },

                    new StoredProcedureParameter
                    {
                        Name = "@user_id",
                        Value = userId
                    });

            if (employeeResult <= 0)
            {
                throw new InvalidOperationException(
                    "Employee record not found.");
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
