using StudentManagement.Application.Interfaces.Repositories.Registration;
using StudentManagement.Application.Interfaces.Services;
using StudentManagement.Application.Interfaces.Services.Registration;
using StudentManagement.Domain.Entities.Registration;
using System.Security.Cryptography;

namespace StudentManagement.Application.Services.Registration
{
    public class StudentDetailsService : IStudentDetailsService
    {
        private readonly IStudentDetailsRepository _repository;
        private readonly IEmailService _emailService;

        public StudentDetailsService(
            IStudentDetailsRepository repository,
            IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }

        // =====================================================
        // GET BY ID
        // =====================================================
        public async Task<StudentDetails?> GetByIdAsync(int studentId)
        {
            return await _repository.GetByIdAsync(studentId);
        }

        // =====================================================
        // GET ALL
        // =====================================================
        public async Task<IEnumerable<StudentDetails>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        // =====================================================
        // CREATE STUDENT
        // =====================================================
        public async Task<StudentDetails> AddAsync(StudentDetails student)
        {
            if (student == null)
            {
                throw new ArgumentException(
                    "Student data is required.");
            }

            // Get existing active students
            var students = await _repository.GetAllAsync();

            // =====================================================
            // EMAIL VALIDATION
            // =====================================================
            if (string.IsNullOrWhiteSpace(student.EmailAddress))
            {
                throw new ArgumentException(
                    "Email address is required.");
            }

            bool emailExists = students.Any(x =>
                !string.IsNullOrWhiteSpace(x.EmailAddress) &&
                x.EmailAddress.Equals(
                    student.EmailAddress,
                    StringComparison.OrdinalIgnoreCase));

            if (emailExists)
            {
                throw new InvalidOperationException(
                    "A student with this email address already exists.");
            }

            // =====================================================
            // MOBILE VALIDATION
            // =====================================================
            if (string.IsNullOrWhiteSpace(student.MobileNumber))
            {
                throw new ArgumentException(
                    "Mobile number is required.");
            }

            if (!student.MobileNumber.All(char.IsDigit))
            {
                throw new ArgumentException(
                    "Mobile number must contain only digits.");
            }

            if (student.MobileNumber.Length != 10)
            {
                throw new ArgumentException(
                    "Mobile number must contain exactly 10 digits.");
            }

            bool mobileExists = students.Any(x =>
                !string.IsNullOrWhiteSpace(x.MobileNumber) &&
                x.MobileNumber == student.MobileNumber);

            if (mobileExists)
            {
                throw new InvalidOperationException(
                    "A student with this mobile number already exists.");
            }

            // =====================================================
            // AADHAR VALIDATION
            // =====================================================
            if (string.IsNullOrWhiteSpace(
                student.AadharCardNumber))
            {
                throw new ArgumentException(
                    "Aadhar card number is required.");
            }

            if (!student.AadharCardNumber.All(char.IsDigit))
            {
                throw new ArgumentException(
                    "Aadhar card number must contain only digits.");
            }

            if (student.AadharCardNumber.Length != 12)
            {
                throw new ArgumentException(
                    "Aadhar card number must contain exactly 12 digits.");
            }

            bool aadharExists = students.Any(x =>
                !string.IsNullOrWhiteSpace(x.AadharCardNumber) &&
                x.AadharCardNumber ==
                student.AadharCardNumber);

            if (aadharExists)
            {
                throw new InvalidOperationException(
                    "A student with this Aadhar card number already exists.");
            }

            // =====================================================
            // PERMANENT IDENTIFICATION NUMBER VALIDATION
            // =====================================================
            if (string.IsNullOrWhiteSpace(
                student.PermanentIdentificationNumber))
            {
                throw new ArgumentException(
                    "Permanent identification number is required.");
            }

            if (!student.PermanentIdentificationNumber.All(
                char.IsDigit))
            {
                throw new ArgumentException(
                    "Permanent identification number must contain only digits.");
            }

            if (student.PermanentIdentificationNumber.Length != 15)
            {
                throw new ArgumentException(
                    "Permanent identification number must contain exactly 15 digits.");
            }

            bool identificationExists = students.Any(x =>
                !string.IsNullOrWhiteSpace(
                    x.PermanentIdentificationNumber) &&
                x.PermanentIdentificationNumber ==
                student.PermanentIdentificationNumber);

            if (identificationExists)
            {
                throw new InvalidOperationException(
                    "A student with this permanent identification number already exists.");
            }

            // =====================================================
            // GENERATE STUDENT CODE
            // =====================================================
            var lastStudentCode = students
                .Where(x =>
                    !string.IsNullOrWhiteSpace(x.StudentCode))
                .Select(x => x.StudentCode!)
                .Where(x =>
                    x.StartsWith(
                        "CTIS",
                        StringComparison.OrdinalIgnoreCase))
                .Select(x =>
                {
                    var numberPart = x.Substring(4);

                    return int.TryParse(
                        numberPart,
                        out int number)
                        ? number
                        : 0;
                })
                .DefaultIfEmpty(0)
                .Max();

            int nextNumber = lastStudentCode + 1;

            student.StudentCode =
                $"CTIS{nextNumber:D4}";

            // =====================================================
            // GENERATE PASSWORD
            // =====================================================
            string generatedPassword =
                GeneratePassword();

            student.Password =
                generatedPassword;

            // =====================================================
            // INSERT STUDENT
            // =====================================================
            var result =
                await _repository.AddAsync(student);

            // =====================================================
            // SEND REGISTRATION EMAIL
            // =====================================================
            await _emailService.SendRegistrationEmailAsync(
                student.EmailAddress,
                student.StudentCode,
                generatedPassword,
                student.StudentName ?? "Student");

            return result;
        }

        // =====================================================
        // UPDATE
        // =====================================================
        public async Task UpdateAsync(StudentDetails student)
        {
            if (student == null)
            {
                throw new ArgumentException(
                    "Student data is required.");
            }

            if (student.StudentId <= 0)
            {
                throw new ArgumentException(
                    "StudentId is required.");
            }

            await _repository.UpdateAsync(student);
        }

        // =====================================================
        // DELETE
        // =====================================================
        public async Task DeleteAsync(int studentId)
        {
            if (studentId <= 0)
            {
                throw new ArgumentException(
                    "Invalid StudentId.");
            }

            await _repository.DeleteAsync(studentId);
        }

        // =====================================================
        // RESTORE
        // =====================================================
        public async Task RestoreAsync(int studentId)
        {
            if (studentId <= 0)
            {
                throw new ArgumentException(
                    "Invalid StudentId.");
            }

            await _repository.RestoreAsync(studentId);
        }

        // =====================================================
        // GENERATE RANDOM PASSWORD
        // =====================================================
        private static string GeneratePassword()
        {
            const string upper =
                "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            const string lower =
                "abcdefghijklmnopqrstuvwxyz";

            const string numbers =
                "0123456789";

            const string special =
                "@#$!";

            string allCharacters =
                upper + lower + numbers + special;

            var password = new char[10];

            password[0] =
                upper[
                    RandomNumberGenerator.GetInt32(
                        upper.Length)];

            password[1] =
                lower[
                    RandomNumberGenerator.GetInt32(
                        lower.Length)];

            password[2] =
                numbers[
                    RandomNumberGenerator.GetInt32(
                        numbers.Length)];

            password[3] =
                special[
                    RandomNumberGenerator.GetInt32(
                        special.Length)];

            for (int i = 4; i < password.Length; i++)
            {
                password[i] =
                    allCharacters[
                        RandomNumberGenerator.GetInt32(
                            allCharacters.Length)];
            }

            // Shuffle password
            return new string(
                password
                    .OrderBy(_ =>
                        RandomNumberGenerator.GetInt32(
                            int.MaxValue))
                    .ToArray());
        }
    }
}