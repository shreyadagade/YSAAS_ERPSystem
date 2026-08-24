using StudentManagement.Application.DTOs.Login;
using StudentManagement.Application.Interfaces.Repositories.Student;
using StudentManagement.Application.Interfaces.Services.Login;

namespace StudentManagement.Application.Services.Student
{
    public class StudentLoginService : IStudentLoginService
    {
        private readonly IStudentDetailsRepository _studentRepository;

        public StudentLoginService(
            IStudentDetailsRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        // =====================================================
        // STUDENT LOGIN
        // =====================================================
        public async Task<StudentLoginResponseDto> LoginAsync(
            StudentLoginRequestDto request)
        {
            // =================================================
            // VALIDATION
            // =================================================

            if (request == null)
            {
                throw new ArgumentException(
                    "Login data is required.");
            }

            if (string.IsNullOrWhiteSpace(request.StudentCode))
            {
                throw new ArgumentException(
                    "Student code is required.");
            }

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                throw new ArgumentException(
                    "Password is required.");
            }

            // =================================================
            // FIND STUDENT BY STUDENT CODE
            // =================================================

            var student =
                await _studentRepository.GetByStudentCodeAsync(
                    request.StudentCode.Trim());

            if (student == null)
            {
                throw new UnauthorizedAccessException(
                    "Invalid student code or password.");
            }

            // =================================================
            // CHECK STORED PASSWORD HASH
            // =================================================

            if (string.IsNullOrWhiteSpace(student.Password))
            {
                throw new UnauthorizedAccessException(
                    "Invalid student code or password.");
            }

            bool passwordValid =
                BCrypt.Net.BCrypt.Verify(
                    request.Password,
                    student.Password);

            if (!passwordValid)
            {
                throw new UnauthorizedAccessException(
                    "Invalid student code or password.");
            }

            // =================================================
            // LOGIN SUCCESS
            // =================================================

            return new StudentLoginResponseDto
            {
                StudentId = student.StudentId,

                StudentCode = student.StudentCode,

                StudentName = student.StudentName,

                EmailAddress = student.EmailAddress,

                Message = "Login successful."
            };
        }
    }
}

