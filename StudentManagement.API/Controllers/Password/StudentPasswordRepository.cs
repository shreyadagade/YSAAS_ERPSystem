using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Application.DTOs.Password;
using StudentManagement.Application.Interfaces.Services.Password;

namespace StudentManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StudentPasswordController : ControllerBase
    {
        private readonly IStudentPasswordService _passwordService;

        public StudentPasswordController(
            IStudentPasswordService passwordService)
        {
            _passwordService = passwordService;
        }

        // =====================================================
        // CHANGE PASSWORD
        // =====================================================

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(
            ChangePasswordRequestDto request)
        {
            // =================================================
            // GET STUDENT ID FROM JWT TOKEN
            // =================================================

            var studentIdClaim =
                User.FindFirst("StudentId")?.Value;

            // DEBUG: Check what StudentId is coming from JWT
            Console.WriteLine(
                $"StudentId from JWT: {studentIdClaim}");

            // =================================================
            // CHECK STUDENT ID
            // =================================================

            if (string.IsNullOrWhiteSpace(studentIdClaim))
            {
                return Unauthorized(new
                {
                    message =
                        "Student ID was not found in token."
                });
            }

            // =================================================
            // CONVERT STUDENT ID TO INTEGER
            // =================================================

            if (!int.TryParse(
                studentIdClaim,
                out int studentId))
            {
                return Unauthorized(new
                {
                    message =
                        "Invalid Student ID in token."
                });
            }

            // =================================================
            // CHANGE PASSWORD
            // =================================================

            await _passwordService.ChangePasswordAsync(
                studentId,
                request);

            // =================================================
            // SUCCESS RESPONSE
            // =================================================

            return Ok(new
            {
                message =
                    "Password changed successfully."
            });
        }
    }
}