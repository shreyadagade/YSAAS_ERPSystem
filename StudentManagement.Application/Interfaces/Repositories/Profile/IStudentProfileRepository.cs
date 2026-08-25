using StudentManagement.Application.DTOs;
using StudentManagement.Application.DTOs.StudentProfile;

namespace StudentManagement.Application.Interfaces.Repositories.Profile
{
    public interface IStudentProfileRepository
    {
        Task<StudentProfileDto?> GetProfileByStudentIdAsync(
            int studentId);
    }
}