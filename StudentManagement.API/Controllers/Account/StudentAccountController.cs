using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Application.DTOs.Password;
using StudentManagement.Application.DTOs.Profile;
using StudentManagement.Application.DTOs.StudentProfile;
using StudentManagement.Application.Interfaces.Services.Password;
using StudentManagement.Application.Interfaces.Services.StudentProfile;

namespace StudentManagement.API.Controllers.Account
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StudentAccountController : ControllerBase
    {
        private readonly IStudentPasswordService _passwordService;
        private readonly IStudentProfileService _profileService;

        public StudentAccountController(
            IStudentPasswordService passwordService,
            IStudentProfileService profileService)
        {
            _passwordService = passwordService;
            _profileService = profileService;
        }

        // =====================================================
        // GET MY PROFILE
        // =====================================================

        [HttpGet("profile")]
        public async Task<IActionResult> GetMyProfile()
        {
            var studentIdClaim =
                User.FindFirst("StudentId")?.Value;

            if (string.IsNullOrWhiteSpace(studentIdClaim))
            {
                return Unauthorized(new
                {
                    message =
                        "Student ID was not found in token."
                });
            }

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

            var profile =
                await _profileService
                    .GetMyProfileAsync(studentId);

            if (profile == null)
            {
                return NotFound(new
                {
                    message =
                        "Student profile not found."
                });
            }

            return Ok(profile);
        }

        // =====================================================
        // CHANGE PASSWORD
        // =====================================================

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(
            ChangePasswordRequestDto request)
        {
            var studentIdClaim =
                User.FindFirst("StudentId")?.Value;

            if (string.IsNullOrWhiteSpace(studentIdClaim))
            {
                return Unauthorized(new
                {
                    message =
                        "Student ID was not found in token."
                });
            }

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

            await _passwordService.ChangePasswordAsync(
                studentId,
                request);

            return Ok(new
            {
                message =
                    "Password changed successfully."
            });
        }

        // =====================================================
        // CHANGE PROFILE
        // =====================================================

        [HttpPut("change-profile")]
        public async Task<IActionResult> ChangeProfile(
            ChangeProfileRequestDto request)
        {
            var studentIdClaim =
                User.FindFirst("StudentId")?.Value;

            if (string.IsNullOrWhiteSpace(studentIdClaim))
            {
                return Unauthorized(new
                {
                    message =
                        "Student ID was not found in token."
                });
            }

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

            var updated =
                await _profileService
                    .ChangeProfileAsync(
                        studentId,
                        request);

            if (!updated)
            {
                return BadRequest(new
                {
                    message =
                        "Profile could not be updated."
                });
            }

            return Ok(new
            {
                message =
                    "Profile updated successfully."
            });
        }

        // =====================================================
        // CHANGE PROFILE PHOTO
        // =====================================================

        [HttpPut("change-profile-photo")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult>
            ChangeProfilePhoto(
                IFormFile profilePhoto)
        {
            var studentIdClaim =
                User.FindFirst("StudentId")?.Value;

            if (string.IsNullOrWhiteSpace(studentIdClaim))
            {
                return Unauthorized(new
                {
                    message =
                        "Student ID was not found in token."
                });
            }

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

            if (profilePhoto == null ||
                profilePhoto.Length == 0)
            {
                return BadRequest(new
                {
                    message =
                        "Profile photo is required."
                });
            }

            // =================================================
            // VALIDATE FILE TYPE
            // =================================================

            var allowedExtensions =
                new[] { ".jpg", ".jpeg", ".png" };

            var extension =
                Path.GetExtension(
                    profilePhoto.FileName)
                    .ToLowerInvariant();

            if (!allowedExtensions.Contains(
                extension))
            {
                return BadRequest(new
                {
                    message =
                        "Only JPG, JPEG and PNG files are allowed."
                });
            }

            // =================================================
            // CREATE UPLOAD DIRECTORY
            // =================================================

            var uploadsFolder =
                Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "uploads",
                    "profilephotos");

            if (!Directory.Exists(
                uploadsFolder))
            {
                Directory.CreateDirectory(
                    uploadsFolder);
            }

            // =================================================
            // GENERATE UNIQUE FILE NAME
            // =================================================

            var fileName =
                $"{Guid.NewGuid()}{extension}";

            var filePath =
                Path.Combine(
                    uploadsFolder,
                    fileName);

            // =================================================
            // SAVE FILE
            // =================================================

            using (
                var stream =
                new FileStream(
                    filePath,
                    FileMode.Create))
            {
                await profilePhoto
                    .CopyToAsync(stream);
            }

            // =================================================
            // PATH STORED IN DATABASE
            // =================================================

            var photoPath =
                $"/uploads/profilephotos/{fileName}";

            // =================================================
            // UPDATE DATABASE
            // =================================================

            var updated =
                await _profileService
                    .ChangeProfilePhotoAsync(
                        studentId,
                        photoPath);

            if (!updated)
            {
                // Delete uploaded file if DB update fails
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                return BadRequest(new
                {
                    message =
                        "Profile photo could not be updated."
                });
            }

            return Ok(new
            {
                message =
                    "Profile photo changed successfully.",
                profilePhoto =
                    photoPath
            });
        }
    }
}