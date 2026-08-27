
using StudentManagement.Application.DTOs.Profile;
using StudentManagement.Application.DTOs.StudentProfile;
using StudentManagement.Application.Interfaces.Repositories.Profile;
using StudentManagement.Application.Interfaces.Services.StudentProfile;

namespace StudentManagement.Application.Services.Profile
{
    public class StudentProfileService
        : IStudentProfileService
    {
        private readonly IStudentProfileRepository _repository;

        public StudentProfileService(
            IStudentProfileRepository repository)
        {
            _repository = repository;
        }

        // =====================================================
        // GET MY PROFILE
        // =====================================================

        public async Task<StudentProfileDto?> GetMyProfileAsync(
            int studentId)
        {
            if (studentId <= 0)
            {
                throw new ArgumentException(
                    "Invalid student ID.");
            }

            return await _repository
                .GetProfileByStudentIdAsync(studentId);
        }

        // =====================================================
        // CHANGE PROFILE
        // =====================================================

        public async Task<bool> ChangeProfileAsync(
            int studentId,
            ChangeProfileRequestDto request)
        {
            if (studentId <= 0)
            {
                throw new ArgumentException(
                    "Invalid student ID.");
            }

            if (request == null)
            {
                throw new ArgumentException(
                    "Profile data is required.");
            }

            return await _repository
                .ChangeProfileAsync(
                    studentId,
                    request);
        }

        // =====================================================
        // CHANGE PROFILE PHOTO
        // =====================================================

        public async Task<bool> ChangeProfilePhotoAsync(
            int studentId,
            string profilePhoto)
        {
            if (studentId <= 0)
            {
                throw new ArgumentException(
                    "Invalid student ID.");
            }

            if (string.IsNullOrWhiteSpace(profilePhoto))
            {
                throw new ArgumentException(
                    "Profile photo path is required.");
            }

            return await _repository
                .ChangeProfilePhotoAsync(
                    studentId,
                    profilePhoto);
        }
    }
}

