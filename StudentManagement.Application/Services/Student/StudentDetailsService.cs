using StudentManagement.Application.DTOs.Student;
using StudentManagement.Application.Interfaces.Repositories.Student;
using StudentManagement.Application.Interfaces.Services.Student;
using StudentManagement.Domain.Entities.Student;

namespace StudentManagement.Application.Services.Student
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
        public async Task<StudentDetailsResponseDto?> GetByIdAsync(
            int studentId)
        {
            if (studentId <= 0)
            {
                throw new ArgumentException(
                    "StudentId must be greater than 0.");
            }

            var student =
                await _repository.GetByIdAsync(studentId);

            if (student == null)
            {
                return null;
            }

            return MapToResponse(student);
        }

        // =====================================================
        // GET ALL
        // =====================================================
        public async Task<IEnumerable<StudentDetailsResponseDto>>
            GetAllAsync()
        {
            var students =
                await _repository.GetAllAsync();

            return students.Select(MapToResponse);
        }

        // =====================================================
        // CREATE STUDENT
        // =====================================================
        public async Task<StudentDetailsResponseDto> AddAsync(
            StudentDetailsRequestDto request)
        {
            if (request == null)
            {
                throw new ArgumentException(
                    "Student data is required.");
            }

            ValidateStudent(request);

            // =================================================
            // STEP 1: GENERATE PASSWORD
            // =================================================

            var generatedPassword =
                PasswordGenerator.GeneratePassword();

            // =================================================
            // STEP 2: CREATE STUDENT ENTITY
            // =================================================

            var student = new StudentDetails
            {
                StudentName =
                    request.StudentName,

                Gender =
                    request.Gender,

                MobileNumber =
                    request.MobileNumber,

                EmailAddress =
                    request.EmailAddress,

                Password =
    BCrypt.Net.BCrypt.HashPassword(generatedPassword),

                BirthDate =
                    request.BirthDate,

                ProfilePhoto =
                    request.ProfilePhoto,

                Qualification =
                    request.Qualification,

                ParentName =
                    request.ParentName,

                ParentNumber =
                    request.ParentNumber,

                LastName =
                    request.LastName,

                WhatsappNumber =
                    request.WhatsappNumber,

                LocalAddress =
                    request.LocalAddress,

                PermanentAddress =
                    request.PermanentAddress,

                PermanentIdentificationNumber =
                    request.PermanentIdentificationNumber,

                AadharCardNumber =
                    request.AadharCardNumber,

                AadharCardPhoto =
                    request.AadharCardPhoto,

                BranchId =
                    request.BranchId
            };

            // =================================================
            // STEP 3: INSERT STUDENT
            // =================================================

            var result =
                await _repository.AddAsync(student);

            // At this point StudentId is available.
            // Example: StudentId = 5

            if (result.StudentId <= 0)
            {
                throw new Exception(
                    "Student was created but StudentId was not generated.");
            }

            // =================================================
            // STEP 4: GENERATE STUDENT CODE
            // =================================================

            result.StudentCode =
                StudentCodeGenerator.GenerateStudentCode(
                    result.StudentId);

            // Keep the generated password.
            result.Password =
                generatedPassword;

            // =================================================
            // STEP 5: UPDATE STUDENT CODE AND PASSWORD
            // =================================================

            await _repository.UpdateAsync(result);

            // =================================================
            // STEP 6: SEND EMAIL
            // =================================================

            if (string.IsNullOrWhiteSpace(result.EmailAddress))
            {
                throw new Exception(
                    "Student email address is required to send login credentials.");
            }

            var emailSubject =
                "Student Account Created Successfully";

            var emailBody =
                $"Dear {result.StudentName},\n\n" +
                $"Your student account has been created successfully.\n\n" +
                $"Student Code: {result.StudentCode}\n" +
                $"Email: {result.EmailAddress}\n" +
                $"Password: {generatedPassword}\n\n" +
                $"Please keep these credentials safe.\n\n" +
                $"Regards,\n" +
                $"Student Management Team";

            await _emailService.SendEmailAsync(
                result.EmailAddress,
                emailSubject,
                emailBody);

            // =================================================
            // STEP 7: RETURN RESPONSE
            // =================================================

            return MapToResponse(result);
        }

        // =====================================================
        // UPDATE
        // =====================================================
        public async Task UpdateAsync(
            int studentId,
            StudentDetailsRequestDto request)
        {
            if (studentId <= 0)
            {
                throw new ArgumentException(
                    "StudentId must be greater than 0.");
            }

            if (request == null)
            {
                throw new ArgumentException(
                    "Student data is required.");
            }

            ValidateStudent(request);

            var existing =
                await _repository.GetByIdAsync(studentId);

            if (existing == null)
            {
                throw new KeyNotFoundException(
                    "Student not found.");
            }

            existing.StudentName =
                request.StudentName;

            existing.Gender =
                request.Gender;

            existing.MobileNumber =
                request.MobileNumber;

            existing.EmailAddress =
                request.EmailAddress;

            existing.BirthDate =
                request.BirthDate;

            existing.ProfilePhoto =
                request.ProfilePhoto;

            existing.Qualification =
                request.Qualification;

            existing.ParentName =
                request.ParentName;

            existing.ParentNumber =
                request.ParentNumber;

            existing.LastName =
                request.LastName;

            existing.WhatsappNumber =
                request.WhatsappNumber;

            existing.LocalAddress =
                request.LocalAddress;

            existing.PermanentAddress =
                request.PermanentAddress;

            existing.PermanentIdentificationNumber =
                request.PermanentIdentificationNumber;

            existing.AadharCardNumber =
                request.AadharCardNumber;

            existing.AadharCardPhoto =
                request.AadharCardPhoto;

            existing.BranchId =
                request.BranchId;

            // Do NOT change the existing password
            // during normal student update.

            await _repository.UpdateAsync(existing);
        }

        // =====================================================
        // DELETE
        // =====================================================
        public async Task DeleteAsync(int studentId)
        {
            if (studentId <= 0)
            {
                throw new ArgumentException(
                    "StudentId must be greater than 0.");
            }

            var existing =
                await _repository.GetByIdAsync(studentId);

            if (existing == null)
            {
                throw new KeyNotFoundException(
                    "Student not found.");
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
                    "StudentId must be greater than 0.");
            }

            await _repository.RestoreAsync(studentId);
        }

        // =====================================================
        // VALIDATION
        // =====================================================
        private static void ValidateStudent(
            StudentDetailsRequestDto request)
        {
            // -------------------------------------------------
            // Student Name
            // -------------------------------------------------

            if (string.IsNullOrWhiteSpace(
                request.StudentName))
            {
                throw new ArgumentException(
                    "Student name is required.");
            }

            if (request.StudentName.Length > 100)
            {
                throw new ArgumentException(
                    "Student name cannot exceed 100 characters.");
            }

            // -------------------------------------------------
            // Email
            // -------------------------------------------------

            if (string.IsNullOrWhiteSpace(
                request.EmailAddress))
            {
                throw new ArgumentException(
                    "Email address is required.");
            }

            if (request.EmailAddress.Length > 100)
            {
                throw new ArgumentException(
                    "Email address cannot exceed 100 characters.");
            }

            if (!request.EmailAddress.Contains("@"))
            {
                throw new ArgumentException(
                    "Invalid email address.");
            }

            // -------------------------------------------------
            // Mobile Number
            // -------------------------------------------------

            if (!string.IsNullOrWhiteSpace(
                request.MobileNumber) &&
                request.MobileNumber.Length > 20)
            {
                throw new ArgumentException(
                    "Mobile number cannot exceed 20 characters.");
            }

            // -------------------------------------------------
            // Gender
            // -------------------------------------------------

            if (!string.IsNullOrWhiteSpace(
                request.Gender) &&
                request.Gender.Length > 10)
            {
                throw new ArgumentException(
                    "Gender cannot exceed 10 characters.");
            }

            // -------------------------------------------------
            // Permanent Identification Number
            // -------------------------------------------------

            if (string.IsNullOrWhiteSpace(
                request.PermanentIdentificationNumber))
            {
                throw new ArgumentException(
                    "Permanent identification number is required.");
            }

            if (request.PermanentIdentificationNumber.Length > 15)
            {
                throw new ArgumentException(
                    "Permanent identification number cannot exceed 15 characters.");
            }

            // -------------------------------------------------
            // Branch
            // -------------------------------------------------

            if (request.BranchId.HasValue &&
                request.BranchId.Value <= 0)
            {
                throw new ArgumentException(
                    "BranchId must be greater than 0.");
            }
        }

        // =====================================================
        // ENTITY → RESPONSE DTO
        // =====================================================
        private static StudentDetailsResponseDto MapToResponse(
            StudentDetails student)
        {
            return new StudentDetailsResponseDto
            {
                StudentId =
                    student.StudentId,

                StudentName =
                    student.StudentName,

                Gender =
                    student.Gender,

                MobileNumber =
                    student.MobileNumber,

                EmailAddress =
                    student.EmailAddress,

                BirthDate =
                    student.BirthDate,

                ProfilePhoto =
                    student.ProfilePhoto,

                Qualification =
                    student.Qualification,

                ParentName =
                    student.ParentName,

                ParentNumber =
                    student.ParentNumber,

                StudentCode =
                    student.StudentCode,

                LastName =
                    student.LastName,

                WhatsappNumber =
                    student.WhatsappNumber,

                LocalAddress =
                    student.LocalAddress,

                PermanentAddress =
                    student.PermanentAddress,

                PermanentIdentificationNumber =
                    student.PermanentIdentificationNumber,

                AadharCardNumber =
                    student.AadharCardNumber,

                AadharCardPhoto =
                    student.AadharCardPhoto,

                BranchId =
                    student.BranchId,

                BranchName =
                    student.BranchName
            };
        }
    }
}
