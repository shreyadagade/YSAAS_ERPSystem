using StudentManagement.Application.DTOs.Profile;
using StudentManagement.Application.DTOs.StudentProfile;

namespace StudentManagement.Application.Interfaces.Repositories.Profile
{
    public interface IStudentProfileRepository
    {
        // =====================================================
        // GET PROFILE
        // =====================================================

        Task<StudentProfileDto?> GetProfileByStudentIdAsync(
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