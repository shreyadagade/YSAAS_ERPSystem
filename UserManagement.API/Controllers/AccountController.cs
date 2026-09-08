using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserManagement.Application.DTOs.Account;
using UserManagement.Application.Interfaces;

namespace UserManagement.API.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("change-password")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new
                {
                    statusCode = StatusCodes.Status401Unauthorized,
                    message = "User is not authenticated."
                });
            }

            var result = await _accountService.ChangePasswordAsync(userId, dto);

            return StatusCode(
               StatusCodes.Status200OK,
               new
               {
                   statusCode = StatusCodes.Status200OK,
                   message = result
               });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto)
        {
            var token = await _accountService.ForgotPasswordAsync(dto);

            return StatusCode(
                 StatusCodes.Status200OK,
                 new
                 {
                     statusCode = StatusCodes.Status200OK,
                     message = "Password reset token generated successfully.",
                     token = token
                 });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            var result = await _accountService.ResetPasswordAsync(dto);

            return StatusCode(
               StatusCodes.Status200OK,
               new
               {
                   statusCode = StatusCodes.Status200OK,
                   message = result
               });
        }

    }
}