using StudentManagement.Application.DTOs.Password;
using StudentManagement.Application.Interfaces.Repositories.Password;
using StudentManagement.Application.Interfaces.Services.Password;

namespace StudentManagement.Application.Services.Password
{
    public class StudentPasswordService : IStudentPasswordService
    {
        private readonly IStudentPasswordRepository _passwordRepository;

        public StudentPasswordService(
            IStudentPasswordRepository passwordRepository)
        {
            _passwordRepository = passwordRepository;
        }

        // =====================================================
        // CHANGE PASSWORD
        // =====================================================

        public async Task ChangePasswordAsync(
            int studentId,
            ChangePasswordRequestDto request)
        {
            // =================================================
            // VALIDATION
            // =================================================

            if (studentId <= 0)
            {
                throw new ArgumentException(
                    "Invalid student ID.");
            }

            if (request == null)
            {
                throw new ArgumentException(
                    "Password data is required.");
            }

            if (string.IsNullOrWhiteSpace(
                request.CurrentPassword))
            {
                throw new ArgumentException(
                    "Current password is required.");
            }

            if (string.IsNullOrWhiteSpace(
                request.NewPassword))
            {
                throw new ArgumentException(
                    "New password is required.");
            }

            if (string.IsNullOrWhiteSpace(
                request.ConfirmPassword))
            {
                throw new ArgumentException(
                    "Confirm password is required.");
            }

            // =================================================
            // CHECK NEW PASSWORD CONFIRMATION
            // =================================================

            if (request.NewPassword !=
                request.ConfirmPassword)
            {
                throw new ArgumentException(
                    "New password and confirm password do not match.");
            }

            // =================================================
            // GET EXISTING PASSWORD HASH
            // =================================================

            var storedHash =
                await _passwordRepository
                    .GetPasswordHashByStudentIdAsync(
                        studentId);

            if (string.IsNullOrWhiteSpace(storedHash))
            {
                throw new UnauthorizedAccessException(
                    "Student password not found.");
            }

            // =================================================
            // VERIFY CURRENT PASSWORD
            // =================================================

            bool currentPasswordValid =
                BCrypt.Net.BCrypt.Verify(
                    request.CurrentPassword,
                    storedHash);

            if (!currentPasswordValid)
            {
                throw new UnauthorizedAccessException(
                    "Current password is incorrect.");
            }

            // =================================================
            // HASH NEW PASSWORD
            // =================================================

            string newPasswordHash =
                BCrypt.Net.BCrypt.HashPassword(
                    request.NewPassword);

            // =================================================
            // UPDATE PASSWORD
            // =================================================

            bool updated =
                await _passwordRepository
                    .UpdatePasswordAsync(
                        studentId,
                        newPasswordHash);

            if (!updated)
            {
                throw new Exception(
                    "Password could not be updated.");
            }
        }
    }
}