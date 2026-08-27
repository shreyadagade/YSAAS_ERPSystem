using StudentManagement.Application.DTOs.Student;
using StudentManagement.Application.Helpers;
using StudentManagement.Application.Interfaces.Repositories.Student;
using StudentManagement.Application.Interfaces.Services.Email;
using StudentManagement.Application.Interfaces.Services.Student;
using StudentManagement.Domain.Entities.Student;

namespace StudentManagement.Application.Services.Student
{
    public class StudentDetailsService
        : IStudentDetailsService
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

        public async Task<StudentDetailsResponseDto?>
            GetByIdAsync(int studentId)
        {
            if (studentId <= 0)
            {
                throw new ArgumentException(
                    "StudentId must be greater than 0.");
            }

            var student =
                await _repository.GetByIdAsync(
                    studentId);

            if (student == null)
            {
                return null;
            }

            return MapToResponse(student);
        }

        // =====================================================
        // GET ALL
        // =====================================================

        public async Task<
            IEnumerable<StudentDetailsResponseDto>>
            GetAllAsync()
        {
            var students =
                await _repository.GetAllAsync();

            return students.Select(
                MapToResponse);
        }

        // =====================================================
        // CREATE STUDENT
        // =====================================================

        public async Task<StudentDetailsResponseDto>
            AddAsync(
                StudentDetailsRequestDto request)
        {
            if (request == null)
            {
                throw new ArgumentException(
                    "Student data is required.");
            }

            ValidateStudent(request);

            // =================================================
            // DUPLICATE VALIDATION
            // =================================================

            await ValidateDuplicatesAsync(
                request);

            // =================================================
            // GENERATE PASSWORD
            // =================================================

            var generatedPassword =
                PasswordGenerator.GeneratePassword();

            // =================================================
            // HASH PASSWORD
            // =================================================

            var hashedPassword =
                BCrypt.Net.BCrypt.HashPassword(
                    generatedPassword);

            // =================================================
            // CREATE ENTITY
            // =================================================

            var student =
                new StudentDetails
                {
                    StudentName =
                        request.StudentName?.Trim(),

                    Gender =
                        request.Gender?.Trim(),

                    MobileNumber =
                        request.MobileNumber?.Trim(),

                    EmailAddress =
                        request.EmailAddress?.Trim(),

                    Password =
                        hashedPassword,

                    BirthDate =
                        request.BirthDate,

                    ProfilePhoto =
                        request.ProfilePhoto,

                    Qualification =
                        request.Qualification?.Trim(),

                    ParentName =
                        request.ParentName?.Trim(),

                    ParentNumber =
                        request.ParentNumber?.Trim(),

                    LastName =
                        request.LastName?.Trim(),

                    WhatsappNumber =
                        request.WhatsappNumber?.Trim(),

                    LocalAddress =
                        request.LocalAddress?.Trim(),

                    PermanentAddress =
                        request.PermanentAddress?.Trim(),

                    PermanentIdentificationNumber =
                        request
                            .PermanentIdentificationNumber
                            ?.Trim(),

                    AadharCardNumber =
                        request.AadharCardNumber?.Trim(),

                    AadharCardPhoto =
                        request.AadharCardPhoto,

                    BranchId =
                        request.BranchId
                };

            // =================================================
            // INSERT
            // =================================================

            var result =
                await _repository.AddAsync(
                    student);

            if (result.StudentId <= 0)
            {
                throw new Exception(
                    "Student was created but StudentId was not generated.");
            }

            // =================================================
            // GENERATE STUDENT CODE
            // =================================================

            result.StudentCode =
                StudentCodeGenerator
                    .GenerateStudentCode(
                        result.StudentId);

            // =================================================
            // SAVE STUDENT CODE
            // =================================================

            await _repository.UpdateAsync(
                result);

            // =================================================
            // SEND LOGIN CREDENTIALS
            // =================================================

            if (string.IsNullOrWhiteSpace(
                result.EmailAddress))
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
            // RETURN RESPONSE
            // =================================================

            return MapToResponse(result);
        }

        // =====================================================
        // DUPLICATE VALIDATION
        // =====================================================

        private async Task ValidateDuplicatesAsync(
            StudentDetailsRequestDto request)
        {
            // -------------------------------------------------
            // EMAIL
            // -------------------------------------------------

            if (!string.IsNullOrWhiteSpace(
                request.EmailAddress))
            {
                var exists =
                    await _repository.ExistsByEmailAsync(
                        request.EmailAddress.Trim());

                if (exists)
                {
                    throw new ArgumentException(
                        "Student with this email address already exists.");
                }
            }

            // -------------------------------------------------
            // MOBILE NUMBER
            // -------------------------------------------------

            if (!string.IsNullOrWhiteSpace(
                request.MobileNumber))
            {
                var exists =
                    await _repository.ExistsByMobileNumberAsync(
                        request.MobileNumber.Trim());

                if (exists)
                {
                    throw new ArgumentException(
                        "Student with this mobile number already exists.");
                }
            }

            // -------------------------------------------------
            // WHATSAPP NUMBER
            // -------------------------------------------------

            if (!string.IsNullOrWhiteSpace(
                request.WhatsappNumber))
            {
                var exists =
                    await _repository.ExistsByWhatsappNumberAsync(
                        request.WhatsappNumber.Trim());

                if (exists)
                {
                    throw new ArgumentException(
                        "Student with this WhatsApp number already exists.");
                }
            }

            // -------------------------------------------------
            // PERMANENT IDENTIFICATION NUMBER
            // -------------------------------------------------

            if (!string.IsNullOrWhiteSpace(
                request.PermanentIdentificationNumber))
            {
                var exists =
                    await _repository
                        .ExistsByPermanentIdentificationNumberAsync(
                            request
                                .PermanentIdentificationNumber
                                .Trim());

                if (exists)
                {
                    throw new ArgumentException(
                        "Student with this permanent identification number already exists.");
                }
            }

            // -------------------------------------------------
            // AADHAAR CARD NUMBER
            // -------------------------------------------------

            if (!string.IsNullOrWhiteSpace(
                request.AadharCardNumber))
            {
                var exists =
                    await _repository.ExistsByAadharAsync(
                        request.AadharCardNumber.Trim());

                if (exists)
                {
                    throw new ArgumentException(
                        "Student with this Aadhaar card number already exists.");
                }
            }
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
                await _repository.GetByIdAsync(
                    studentId);

            if (existing == null)
            {
                throw new KeyNotFoundException(
                    "Student not found.");
            }

            // =================================================
            // DUPLICATE VALIDATION FOR UPDATE
            // =================================================

            if (!string.Equals(
                existing.EmailAddress,
                request.EmailAddress,
                StringComparison.OrdinalIgnoreCase))
            {
                var emailExists =
                    await _repository.ExistsByEmailAsync(
                        request.EmailAddress.Trim());

                if (emailExists)
                {
                    throw new ArgumentException(
                        "Student with this email address already exists.");
                }
            }

            if (!string.IsNullOrWhiteSpace(
                request.MobileNumber) &&
                !string.Equals(
                    existing.MobileNumber,
                    request.MobileNumber,
                    StringComparison.OrdinalIgnoreCase))
            {
                var mobileExists =
                    await _repository.ExistsByMobileNumberAsync(
                        request.MobileNumber.Trim());

                if (mobileExists)
                {
                    throw new ArgumentException(
                        "Student with this mobile number already exists.");
                }
            }

            if (!string.IsNullOrWhiteSpace(
                request.WhatsappNumber) &&
                !string.Equals(
                    existing.WhatsappNumber,
                    request.WhatsappNumber,
                    StringComparison.OrdinalIgnoreCase))
            {
                var whatsappExists =
                    await _repository.ExistsByWhatsappNumberAsync(
                        request.WhatsappNumber.Trim());

                if (whatsappExists)
                {
                    throw new ArgumentException(
                        "Student with this WhatsApp number already exists.");
                }
            }

            if (!string.IsNullOrWhiteSpace(
                request.PermanentIdentificationNumber) &&
                !string.Equals(
                    existing.PermanentIdentificationNumber,
                    request.PermanentIdentificationNumber,
                    StringComparison.OrdinalIgnoreCase))
            {
                var permanentIdExists =
                    await _repository
                        .ExistsByPermanentIdentificationNumberAsync(
                            request
                                .PermanentIdentificationNumber
                                .Trim());

                if (permanentIdExists)
                {
                    throw new ArgumentException(
                        "Student with this permanent identification number already exists.");
                }
            }

            if (!string.IsNullOrWhiteSpace(
                request.AadharCardNumber) &&
                !string.Equals(
                    existing.AadharCardNumber,
                    request.AadharCardNumber,
                    StringComparison.OrdinalIgnoreCase))
            {
                var aadharExists =
                    await _repository.ExistsByAadharAsync(
                        request.AadharCardNumber.Trim());

                if (aadharExists)
                {
                    throw new ArgumentException(
                        "Student with this Aadhaar card number already exists.");
                }
            }

            // =================================================
            // UPDATE FIELDS
            // =================================================

            existing.StudentName =
                request.StudentName?.Trim();

            existing.Gender =
                request.Gender?.Trim();

            existing.MobileNumber =
                request.MobileNumber?.Trim();

            existing.EmailAddress =
                request.EmailAddress?.Trim();

            existing.BirthDate =
                request.BirthDate;

            existing.ProfilePhoto =
                request.ProfilePhoto;

            existing.Qualification =
                request.Qualification?.Trim();

            existing.ParentName =
                request.ParentName?.Trim();

            existing.ParentNumber =
                request.ParentNumber?.Trim();

            existing.LastName =
                request.LastName?.Trim();

            existing.WhatsappNumber =
                request.WhatsappNumber?.Trim();

            existing.LocalAddress =
                request.LocalAddress?.Trim();

            existing.PermanentAddress =
                request.PermanentAddress?.Trim();

            existing.PermanentIdentificationNumber =
                request
                    .PermanentIdentificationNumber
                    ?.Trim();

            existing.AadharCardNumber =
                request.AadharCardNumber?.Trim();

            existing.AadharCardPhoto =
                request.AadharCardPhoto;

            existing.BranchId =
                request.BranchId;

            // Password is intentionally not changed.

            await _repository.UpdateAsync(
                existing);
        }

        // =====================================================
        // DELETE
        // =====================================================

        public async Task DeleteAsync(
            int studentId)
        {
            if (studentId <= 0)
            {
                throw new ArgumentException(
                    "StudentId must be greater than 0.");
            }

            var existing =
                await _repository.GetByIdAsync(
                    studentId);

            if (existing == null)
            {
                throw new KeyNotFoundException(
                    "Student not found.");
            }

            await _repository.DeleteAsync(
                studentId);
        }

        // =====================================================
        // RESTORE
        // =====================================================

        public async Task RestoreAsync(
            int studentId)
        {
            if (studentId <= 0)
            {
                throw new ArgumentException(
                    "StudentId must be greater than 0.");
            }

            await _repository.RestoreAsync(
                studentId);
        }

        // =====================================================
        // VALIDATION
        // =====================================================

        private static void ValidateStudent(
            StudentDetailsRequestDto request)
        {
            // -------------------------------------------------
            // STUDENT NAME
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
            // EMAIL
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
            // MOBILE
            // -------------------------------------------------

            if (!string.IsNullOrWhiteSpace(
                request.MobileNumber) &&
                request.MobileNumber.Length > 20)
            {
                throw new ArgumentException(
                    "Mobile number cannot exceed 20 characters.");
            }

            // -------------------------------------------------
            // GENDER
            // -------------------------------------------------

            if (!string.IsNullOrWhiteSpace(
                request.Gender) &&
                request.Gender.Length > 10)
            {
                throw new ArgumentException(
                    "Gender cannot exceed 10 characters.");
            }

            // -------------------------------------------------
            // PERMANENT IDENTIFICATION NUMBER
            // -------------------------------------------------

            if (string.IsNullOrWhiteSpace(
                request.PermanentIdentificationNumber))
            {
                throw new ArgumentException(
                    "Permanent identification number is required.");
            }

            if (request
                .PermanentIdentificationNumber
                .Length > 15)
            {
                throw new ArgumentException(
                    "Permanent identification number cannot exceed 15 characters.");
            }

            // -------------------------------------------------
            // AADHAAR
            // -------------------------------------------------

            if (!string.IsNullOrWhiteSpace(
                request.AadharCardNumber) &&
                request.AadharCardNumber.Length > 100)
            {
                throw new ArgumentException(
                    "Aadhaar card number cannot exceed 100 characters.");
            }

            // -------------------------------------------------
            // WHATSAPP
            // -------------------------------------------------

            if (!string.IsNullOrWhiteSpace(
                request.WhatsappNumber) &&
                request.WhatsappNumber.Length > 15)
            {
                throw new ArgumentException(
                    "WhatsApp number cannot exceed 15 characters.");
            }

            // -------------------------------------------------
            // BRANCH
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

        private static StudentDetailsResponseDto
            MapToResponse(
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