using StudentManagement.Application.DTOs.Profile;
using StudentManagement.Application.DTOs.StudentProfile;

namespace StudentManagement.Application.Interfaces.Repositories.Profile
{
    public interface IStudentProfileRepository
    {
        Task<StudentProfileDto?> GetProfileByStudentIdAsync(
            int studentId);

        Task<bool> ChangeProfileAsync(
            int studentId,
            ChangeProfileRequestDto request);

        Task<bool> ChangeProfilePhotoAsync(
            int studentId,
            string profilePhoto);

        Task<string?> CheckDuplicateAsync(
            int studentId,
            ChangeProfileRequestDto request);
    }
}