using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using UserManagement.Application.DTOs.Account;
using UserManagement.Application.DTOs.Email;
using UserManagement.Application.Exceptions;
using UserManagement.Application.Interfaces;
using UserManagement.Infrastructure.Persistence.Identity;

namespace UserManagement.Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        public AccountService(
            UserManager<ApplicationUser> userManager, IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<string> ChangePasswordAsync(string userId,ChangePasswordDto dto)
        {
            if (dto == null)
            {
                throw new BadRequestException("Request data is required.");
            }

            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new BadRequestException("User ID is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.CurrentPassword))
            {
                throw new BadRequestException("Current password is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.NewPassword))
            {
                throw new BadRequestException("New password is required.");
            }

            if (dto.CurrentPassword == dto.NewPassword)
            {
                throw new BadRequestException(
                    "New password must be different from current password.");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new NotFoundException(
                    "User not found.");
            }

            if (!user.IsActive)
            {
                throw new ForbiddenException(
                    "User account is deactivated.");
            }

            var result =
                await _userManager.ChangePasswordAsync(
                    user,
                    dto.CurrentPassword,
                    dto.NewPassword);

            if (!result.Succeeded)
            {
                var errors =
                    string.Join(
                        ", ",
                        result.Errors.Select(
                            e => e.Description));

                throw new BadRequestException(
                    $"Password change failed. {errors}");
            }

            return "Password changed successfully.";
        }

        public async Task<string> ForgotPasswordAsync(ForgotPasswordDto dto)
        {
            if (dto == null)
            {
                throw new BadRequestException("Request data is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.EmailAddress))
            {
                throw new BadRequestException("Email address is required.");
            }

            var email = dto.EmailAddress.Trim();

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                throw new NotFoundException(
                    "User not found with the provided email address.");
            }

            if (!user.IsActive)
            {
                throw new ForbiddenException(
                    "User account is deactivated.");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var resetToken = $"{user.Id}|{token}";

            return resetToken;
        }

        public async Task<string> ResetPasswordAsync(ResetPasswordDto dto)
        {
            if (dto == null)
            {
                throw new BadRequestException("Request data is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Token))
            {
                throw new BadRequestException("Reset token is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.NewPassword))
            {
                throw new BadRequestException("New password is required.");
            }

            var tokenParts = dto.Token.Split('|', 2);

            if (tokenParts.Length != 2)
            {
                throw new BadRequestException("Invalid reset token.");
            }

            var userId = tokenParts[0];
            var resetToken = tokenParts[1];

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            if (!user.IsActive)
            {
                throw new ForbiddenException(
                    "User account is deactivated.");
            }

            var result = await _userManager.ResetPasswordAsync(
                user,
                resetToken,
                dto.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    result.Errors.Select(e => e.Description));

                throw new BadRequestException(
                    $"Password reset failed. {errors}");
            }

            return "Password reset successfully.";
        }


    }
}

