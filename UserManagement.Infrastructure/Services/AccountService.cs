using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using UserManagement.Application.DTOs.Account;
using UserManagement.Application.DTOs.Email;
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

        public async Task<string> ChangePasswordAsync(ChangePasswordDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserId))
            {
                throw new ArgumentException(
                    "User ID is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.CurrentPassword))
            {
                throw new ArgumentException(
                    "Current password is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.NewPassword))
            {
                throw new ArgumentException(
                    "New password is required.");
            }

            if (dto.CurrentPassword == dto.NewPassword)
            {
                throw new ArgumentException(
                    "New password must be different from current password.");
            }

            var user =
                await _userManager.FindByIdAsync(
                    dto.UserId);

            if (user == null)
            {
                throw new InvalidOperationException(
                    "User not found.");
            }

            if (!user.IsActive)
            {
                throw new InvalidOperationException(
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

                throw new InvalidOperationException(
                    $"Password change failed. {errors}");
            }

            return "Password changed successfully.";
        }

        public async Task<string> ForgotPasswordAsync(ForgotPasswordDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentException(
                    "Request data is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.EmailAddress))
            {
                throw new ArgumentException(
                    "Email address is required.");
            }

            var email = dto.EmailAddress.Trim();

            var user =
                await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return "Please check the email address and try again.";
            }

            if (!user.IsActive)
            {
                return "User account is deactivated.";
            }

            var token =
                await _userManager.GeneratePasswordResetTokenAsync(user);

            return token;
        }

        public async Task<string> ResetPasswordAsync(ResetPasswordDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentException(
                    "Request data is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.EmailAddress))
            {
                throw new ArgumentException(
                    "Email address is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Token))
            {
                throw new ArgumentException(
                    "Reset token is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.NewPassword))
            {
                throw new ArgumentException(
                    "New password is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.ConfirmPassword))
            {
                throw new ArgumentException(
                    "Confirm password is required.");
            }

            if (dto.NewPassword != dto.ConfirmPassword)
            {
                throw new ArgumentException(
                    "New password and confirm password do not match.");
            }

            var user = await _userManager.FindByEmailAsync(
                dto.EmailAddress.Trim());

            if (user == null)
            {
                throw new InvalidOperationException(
                    "Invalid password reset request.");
            }

            if (!user.IsActive)
            {
                throw new InvalidOperationException(
                    "User account is deactivated.");
            }

            var result = await _userManager.ResetPasswordAsync(
                user,
                dto.Token,
                dto.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    result.Errors.Select(e => e.Description));

                throw new InvalidOperationException(
                    $"Password reset failed. {errors}");
            }

            return "Password reset successfully.";
        }

        
    }
}

