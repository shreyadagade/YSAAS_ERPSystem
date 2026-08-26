using StudentManagement.Application.DTOs.Profile;
using StudentManagement.Application.DTOs.StudentProfile;

namespace StudentManagement.Application.Interfaces.Services.StudentProfile
{
    public interface IStudentProfileService
    {
        // =====================================================
        // GET MY PROFILE
        // =====================================================

        Task<StudentProfileDto?> GetMyProfileAsync(
            int studentId);

        // =====================================================
        // CHANGE PROFILE
        // =====================================================

        Task<bool> ChangeProfileAsync(
            int studentId,
            ChangeProfileRequestDto request);

        // =====================================================
        // CHANGE PROFILE PHOTO
        // =====================================================

        Task<bool> ChangeProfilePhotoAsync(
            int studentId,
            string profilePhoto);
    }
}