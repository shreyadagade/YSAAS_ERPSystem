using StudentManagement.Application.DTOs.Login;
using StudentManagement.Application.Interfaces.Repositories.Student;
using StudentManagement.Application.Interfaces.Services.Login;

namespace StudentManagement.Application.Services.Student
{
    public class StudentLoginService : IStudentLoginService
    {
        private readonly IStudentDetailsRepository _studentRepository;
        private readonly IJwtService _jwtService;

        public StudentLoginService(
            IStudentDetailsRepository studentRepository,
            IJwtService jwtService)
        {
            _studentRepository = studentRepository;
            _jwtService = jwtService;
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
            // FIND STUDENT
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
            // CHECK PASSWORD HASH
            // =================================================

            if (string.IsNullOrWhiteSpace(student.Password))
            {
                throw new UnauthorizedAccessException(
                    "Student password hash is missing.");
            }

            Console.WriteLine(
                "==========================================");

            Console.WriteLine(
                $"Student ID: {student.StudentId}");

            Console.WriteLine(
                $"Student Code: {student.StudentCode}");

            Console.WriteLine(
                $"Password Hash Exists: " +
                $"{!string.IsNullOrWhiteSpace(student.Password)}");

            Console.WriteLine(
                $"Password Hash Length: " +
                $"{student.Password.Length}");

            // =================================================
            // VERIFY PASSWORD
            // =================================================

            bool passwordValid;

            try
            {
                passwordValid =
                    BCrypt.Net.BCrypt.Verify(
                        request.Password,
                        student.Password);
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"BCrypt Error: {ex.Message}");

                throw new UnauthorizedAccessException(
                    "Password verification failed.");
            }

            Console.WriteLine(
                $"Password Valid: {passwordValid}");

            Console.WriteLine(
                "==========================================");

            if (!passwordValid)
            {
                throw new UnauthorizedAccessException(
                    "Invalid student code or password.");
            }

            // =================================================
            // GENERATE JWT TOKEN
            // =================================================

            var token =
                _jwtService.GenerateToken(
                    student.StudentId,
                    student.StudentCode!);

            // =================================================
            // LOGIN SUCCESS
            // =================================================

            return new StudentLoginResponseDto
            {
                StudentId =
                    student.StudentId,

                StudentCode =
                    student.StudentCode,

                StudentName =
                    student.StudentName,

                EmailAddress =
                    student.EmailAddress,

                Token =
                    token,

                Message =
                    "Login successful."
            };
        }
    }
}