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
                return "If the email address is registered, a password reset link has been sent.";
            }

            if (!user.IsActive)
            {
                return "If the email address is registered, a password reset link has been sent.";
            }

            var token =
                await _userManager.GeneratePasswordResetTokenAsync(user);

            var encodedEmail =
                Uri.EscapeDataString(email);

            var encodedToken =
                Uri.EscapeDataString(token);

            var resetLink =
                $"https://localhost:7101/api/account/reset-password" +
                $"?email={encodedEmail}&token={encodedToken}";

            var emailRequest = new EmailRequestDto
            {
                ToEmail = email,
                Subject = "CIIT ERP - Password Reset Request",

                Body = $@"
            <h2>CIIT ERP Password Reset</h2>

            <p>Hello,</p>

            <p>
                We received a request to reset your password.
            </p>

            <p>
                Click the link below to reset your password:
            </p>

            <p>
                <a href=""{resetLink}"">
                    Reset Password
                </a>
            </p>

            <p>
                If you did not request this, please ignore this email.
            </p>

            <p>
                Regards,<br/>
                CIIT ERP System
            </p>"
            };

            await _emailService.SendEmailAsync(emailRequest);

            return "If the email address is registered, a password reset link has been sent.";
        }

        public async Task<string> ResetPasswordAsync(ResetPasswordDto dto)
        {
        
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

            var user =
                await _userManager.FindByEmailAsync(
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

            var result =
                await _userManager.ResetPasswordAsync(
                    user,
                    dto.Token,
                    dto.NewPassword);

            if (!result.Succeeded)
            {
                var errors =
                    string.Join(
                        ", ",
                        result.Errors.Select(
                            e => e.Description));

                throw new InvalidOperationException(
                    $"Password reset failed. {errors}");
            }

            return "Password reset successfully.";
        }

    }
}

