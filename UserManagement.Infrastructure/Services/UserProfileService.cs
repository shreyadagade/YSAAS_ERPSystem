using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UserManagement.Application.Contracts;
using UserManagement.Application.DTOs.User;
using UserManagement.Application.Exceptions;
using UserManagement.Application.Interfaces;
using UserManagement.Infrastructure.Persistence.Identity;
using UserManagement.Infrastructure.Persistence.Models;

namespace UserManagement.Infrastructure.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGenericRepository _repository;

        private const string StoredProcedure = "erpsystem.sp_tblemployees";

        public UserProfileService(UserManager<ApplicationUser> userManager,
            IGenericRepository repository)
        {
            _userManager = userManager;
            _repository = repository;
        }
        public async Task<ProfileResponseDto> GetProfileAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new BadRequestException("User ID is required.");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            var result = await _repository.ExecuteQueryAsync<ProfileResponseDto>(
                StoredProcedure,
                new StoredProcedureParameter
                {
                    Name = "@Type",
                    Value = "GetProfile"
                },
                new StoredProcedureParameter
                {
                    Name = "@user_id",
                    Value = userId
                });

            var profile = result.FirstOrDefault();

            if (profile == null)
            {
                throw new NotFoundException("User profile not found.");
            }

            var roles = await _userManager.GetRolesAsync(user);

            profile.IsActive = user.IsActive;
            profile.Roles = roles.ToList();

            return profile;
        }

        public async Task<string> UpdateProfileAsync(string userId, UpdateProfileDto dto)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new BadRequestException(
                    "User ID is required.");
            }

            if (dto == null)
            {
                throw new BadRequestException(
                    "Update data is required.");
            }

            var user = await _userManager.FindByIdAsync(userId);

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

            if (!string.IsNullOrWhiteSpace(dto.EmailAddress))
            {
                var existingUser =
                    await _userManager.FindByEmailAsync(
                        dto.EmailAddress.Trim());

                if (existingUser != null &&
                    existingUser.Id != user.Id)
                {
                    throw new InternalServerErrorException(
                        "Email address is already registered.");
                }

                user.Email = dto.EmailAddress.Trim();

                user.NormalizedEmail =
                    dto.EmailAddress.Trim().ToUpper();
            }

            if (!string.IsNullOrWhiteSpace(dto.MobileNumber))
            {
                var existingMobile =
                    await _userManager.Users.FirstOrDefaultAsync(u =>
                        u.PhoneNumber == dto.MobileNumber.Trim() &&
                        u.Id != user.Id);

                if (existingMobile != null)
                {
                    throw new InternalServerErrorException(
                        "A user with this mobile number already exists.");
                }

                user.PhoneNumber =
                    dto.MobileNumber.Trim();
            }

            var identityResult =
                await _userManager.UpdateAsync(user);

            if (!identityResult.Succeeded)
            {
                var errors =
                    string.Join(
                        ", ",
                        identityResult.Errors.Select(
                            e => e.Description));

                throw new BadRequestException(
                    $"User profile update failed. {errors}");
            }

            var employeeResult =
                await _repository.ExecuteNonQueryAsync(
                    StoredProcedure,

                    new StoredProcedureParameter
                    {
                        Name = "@Type",
                        Value = "UpdateProfile"
                    },

                    new StoredProcedureParameter
                    {
                        Name = "@user_id",
                        Value = userId
                    },

                    new StoredProcedureParameter
                    {
                        Name = "@employee_name",
                        Value = dto.EmployeeName
                    },

                    new StoredProcedureParameter
                    {
                        Name = "@email_address",
                        Value = dto.EmailAddress
                    },

                    new StoredProcedureParameter
                    {
                        Name = "@mobile_number",
                        Value = dto.MobileNumber
                    },

                    new StoredProcedureParameter
                    {
                        Name = "@birth_date",
                        Value = dto.BirthDate
                    },

                    new StoredProcedureParameter
                    {
                        Name = "@qualification",
                        Value = dto.Qualification
                    },

                    new StoredProcedureParameter
                    { 
                        Name = "@joining_date",
                        Value = dto.JoiningDate 
                    },

                    new StoredProcedureParameter 
                    { 
                        Name = "@salary", 
                        Value = dto.Salary 
                    },

                    new StoredProcedureParameter
                    {
                        Name = "@gender",
                        Value = dto.Gender
                    },

                    new StoredProcedureParameter
                    {
                        Name = "@aadhar_card_number",
                        Value = dto.AadharCardNumber
                    },

                    new StoredProcedureParameter
                    {
                        Name = "@pan_number",
                        Value = dto.PanNumber
                    },

                    new StoredProcedureParameter
                    {
                        Name = "@local_address",
                        Value = dto.LocalAddress
                    });

            if (employeeResult <= 0)
            {
                throw new NotFoundException(
                    "User profile record not found.");
            }

            return "User profile updated successfully.";
        }

    }
}
