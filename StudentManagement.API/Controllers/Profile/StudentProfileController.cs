using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Application.Interfaces.Services.StudentProfile;
using System.Security.Claims;

namespace StudentManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StudentProfileController : ControllerBase
    {
        private readonly IStudentProfileService _profileService;

        public StudentProfileController(
            IStudentProfileService profileService)
        {
            _profileService = profileService;
        }

        // =====================================================
        // GET LOGGED-IN STUDENT PROFILE
        // =====================================================

        [HttpGet("profile")]
        public async Task<IActionResult> GetMyProfile()
        {
            // Get StudentId from JWT
            var studentIdClaim =
                User.FindFirst("StudentId")?.Value;

            if (string.IsNullOrWhiteSpace(studentIdClaim))
            {
                return Unauthorized(
                    new
                    {
                        message =
                            "Student ID was not found in the token."
                    });
            }

            if (!int.TryParse(
                    studentIdClaim,
                    out int studentId))
            {
                return Unauthorized(
                    new
                    {
                        message =
                            "Invalid Student ID in token."
                    });
            }

            // Get logged-in student's profile
            var profile =
                await _profileService
                    .GetMyProfileAsync(studentId);

            if (profile == null)
            {
                return NotFound(
                    new
                    {
                        message =
                            "Student profile not found."
                    });
            }

            return Ok(profile);
        }
    }
}