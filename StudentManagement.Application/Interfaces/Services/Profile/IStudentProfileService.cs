using Microsoft.AspNetCore.Http;
using StudentManagement.Application.DTOs.Profile;
using StudentManagement.Application.DTOs.StudentProfile;

namespace StudentManagement.Application.Interfaces.Services.StudentProfile
{
    public interface IStudentProfileService
    {
        Task<StudentProfileDto?> GetMyProfileAsync(
            int studentId);

        Task<bool> ChangeProfileAsync(
            int studentId,
            ChangeProfileRequestDto request);

        Task<bool> ChangeProfilePhotoAsync(
            int studentId,
            string profilePhoto);
    }
}

