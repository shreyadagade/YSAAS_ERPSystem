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
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            var result = await _accountService.ForgotPasswordAsync(email);

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

    }
}