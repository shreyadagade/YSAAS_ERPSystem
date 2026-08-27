using Microsoft.AspNetCore.Mvc;
using StudentManagement.Application.DTOs.ForgotPassword;
using StudentManagement.Application.Interfaces.Services.ForgotPassword;

namespace StudentManagement.API.Controllers.ForgotPassword
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentForgotPasswordController
        : ControllerBase
    {
        private readonly IForgotPasswordService _service;

        public StudentForgotPasswordController(
            IForgotPasswordService service)
        {
            _service = service;
        }

        // =====================================================
        // FORGOT PASSWORD - GENERATE RESET TOKEN
        // =====================================================

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(
            ForgotPasswordRequestDto request)
        {
            var resetToken =
                await _service.GenerateResetTokenAsync(
                    request);

            return Ok(new
            {
                message =
                    "Password reset token generated successfully.",

                resetToken =
                    resetToken
            });
        }

        // =====================================================
        // RESET PASSWORD
        // =====================================================

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(
            ResetPasswordRequestDto request)
        {
            await _service.ResetPasswordAsync(
                request);

            return Ok(new
            {
                message =
                    "Password reset successfully."
            });
        }
    }
}