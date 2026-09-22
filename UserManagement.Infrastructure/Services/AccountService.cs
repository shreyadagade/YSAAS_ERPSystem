using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using UserManagement.Application.Configuration;
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
        private readonly EmailSettings _emailSettings;
        public AccountService(
            UserManager<ApplicationUser> userManager,
            IEmailService emailService,
            IOptions<EmailSettings> emailSettings)
        {
            _userManager = userManager;
            _emailService = emailService;
            _emailSettings = emailSettings.Value;
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

            var encodedToken = Uri.EscapeDataString(token);

            var resetLink =
                $"{_emailSettings.FrontendUrl.TrimEnd('/')}/reset-password" +
                $"?email={Uri.EscapeDataString(user.Email!)}" +
                $"&token={encodedToken}";

            var emailRequest = new EmailRequestDto
            {
                ToEmail = user.Email!,
                Subject = "Reset Your Password",
                Body = $"""
           <p>Hello,</p>

           <p>You requested to reset your password.</p>

           <p>
               <a href="{resetLink}">
                   Click here to reset your password
               </a>
           </p>

           <p>If you did not request this, please ignore this email.</p>
           """
            };

            await _emailService.SendEmailAsync(emailRequest);

            return "Password reset link has been sent to your email address.";
        }

        public async Task<string> ResetPasswordAsync(ResetPasswordDto dto)
        {
            if (dto == null)
            {
                throw new BadRequestException("Request data is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.EmailAddress))
            {
                throw new BadRequestException("Email address is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Token))
            {
                throw new BadRequestException("Reset token is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.NewPassword))
            {
                throw new BadRequestException("New password is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.ConfirmPassword))
            {
                throw new BadRequestException("Confirm password is required.");
            }

            if (dto.NewPassword != dto.ConfirmPassword)
            {
                throw new BadRequestException(
                    "New password and confirm password do not match.");
            }

            var email = dto.EmailAddress.Trim();

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            if (!user.IsActive)
            {
                throw new ForbiddenException(
                    "User account is deactivated.");
            }

            var resetToken = Uri.UnescapeDataString(dto.Token);

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

