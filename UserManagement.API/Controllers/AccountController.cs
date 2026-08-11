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
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var result = await _accountService.ChangePasswordAsync(dto);

            return Ok(new
            {
                message = result
            });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var result = await _accountService.ForgotPasswordAsync(dto);

            return Ok(new
            {
                message = result
            });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var result = await _accountService.ResetPasswordAsync(dto);

            return Ok(new
            {
                message = result
            });
        }

        [HttpGet("reset-password")]
        public IActionResult ResetPasswordLink([FromQuery] string email,
            [FromQuery] string token)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest("Email is required.");
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                return BadRequest("Reset token is required.");
            }

            return Ok(new
            {
                message = "Reset link is valid. Use the POST reset-password API in Swagger.",
                email = email,
                token = token
            });
        }

    }
}