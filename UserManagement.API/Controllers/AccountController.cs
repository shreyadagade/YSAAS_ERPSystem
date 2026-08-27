using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            var result = await _accountService.ChangePasswordAsync(dto);

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