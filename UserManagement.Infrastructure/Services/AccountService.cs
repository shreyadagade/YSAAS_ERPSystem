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

       
        public async Task<string> ForgotPasswordAsync(string email)
        {
            // 1. Validate email
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException(
                    "Email address is required.");
            }

            email = email.Trim();

            // 2. Find user
            var user =
                await _userManager.FindByEmailAsync(email);

            // 3. Do not reveal whether email exists
            if (user == null)
            {
                return "If the email address is registered, a password reset link has been sent.";
            }

            // 4. Check active user
            if (!user.IsActive)
            {
                return "If the email address is registered, a password reset link has been sent.";
            }

            // 5. Generate password reset token
            var token =
                await _userManager.GeneratePasswordResetTokenAsync(
                    user);

            // 6. Encode email and token
            var encodedEmail =
                Uri.EscapeDataString(email);

            var encodedToken =
                Uri.EscapeDataString(token);

            // 7. Create reset link
            var resetLink =
                $"https://localhost:7101/api/account/reset-password" +
                $"?email={encodedEmail}&token={encodedToken}";

            // 8. Create email request
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
                If you did not request a password reset,
                please ignore this email.
            </p>

            <p>
                Regards,<br/>
                CIIT ERP System
            </p>"
            };

            // 9. Send email
            await _emailService.SendEmailAsync(
                emailRequest);

            // 10. Return success
            return "If the email address is registered, a password reset link has been sent.";
        }

        public async Task<string> ResetPasswordAsync(ResetPasswordDto dto)
        {
            // 1. Validate email
            if (string.IsNullOrWhiteSpace(dto.EmailAddress))
            {
                throw new ArgumentException(
                    "Email address is required.");
            }

            // 2. Validate reset token
            if (string.IsNullOrWhiteSpace(dto.Token))
            {
                throw new ArgumentException(
                    "Reset token is required.");
            }

            // 3. Validate new password
            if (string.IsNullOrWhiteSpace(dto.NewPassword))
            {
                throw new ArgumentException(
                    "New password is required.");
            }

            // 4. Find user
            var user =
                await _userManager.FindByEmailAsync(
                    dto.EmailAddress.Trim());

            if (user == null)
            {
                throw new InvalidOperationException(
                    "Invalid password reset request.");
            }

            // 5. Check active user
            if (!user.IsActive)
            {
                throw new InvalidOperationException(
                    "User account is deactivated.");
            }

            // 6. Reset password
            var result =
                await _userManager.ResetPasswordAsync(
                    user,
                    dto.Token,
                    dto.NewPassword);

            // 7. Handle Identity errors
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

            // 8. Success
            return "Password reset successfully.";
        }

    }
}

