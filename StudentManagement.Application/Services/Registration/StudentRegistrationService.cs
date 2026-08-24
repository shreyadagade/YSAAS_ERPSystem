using StudentManagement.Application.DTOs.Registration;
using StudentManagement.Application.Interfaces.Repositories.Registration;
using StudentManagement.Application.Interfaces.Repositories.Student;
using StudentManagement.Application.Interfaces.Services.Registration;
using StudentManagement.Application.Interfaces.Services.Student;
using StudentManagement.Domain.Entities.Registration;
using System.Xml.Linq;

namespace StudentManagement.Application.Services.Registration
{
    public class StudentRegistrationService
        : IStudentRegistrationService
    {
        private readonly IStudentRegistrationRepository _repository;
        private readonly IStudentDetailsRepository _studentRepository;
        private readonly IEmailService _emailService;

        public StudentRegistrationService(
            IStudentRegistrationRepository repository,
            IStudentDetailsRepository studentRepository,
            IEmailService emailService)
        {
            _repository = repository;
            _studentRepository = studentRepository;
            _emailService = emailService;
        }

        // =====================================================
        // GET BY ID
        // =====================================================
        public async Task<StudentRegistrationResponseDto?>
            GetByIdAsync(int registrationId)
        {
            if (registrationId <= 0)
            {
                throw new ArgumentException(
                    "RegistrationId must be greater than 0.");
            }

            var registration =
                await _repository.GetByIdAsync(registrationId);

            if (registration == null)
            {
                return null;
            }

            return MapToResponse(registration);
        }

        // =====================================================
        // GET ALL
        // =====================================================
        public async Task<IEnumerable<StudentRegistrationResponseDto>>
            GetAllAsync()
        {
            var registrations =
                await _repository.GetAllAsync();

            return registrations.Select(MapToResponse);
        }

        // =====================================================
        // CREATE REGISTRATION
        // =====================================================
        public async Task<StudentRegistrationResponseDto>
            AddAsync(StudentRegistrationRequestDto request)
        {
            if (request == null)
            {
                throw new ArgumentException(
                    "Registration data is required.");
            }

            ValidateRegistration(request);

            // =================================================
            // CHECK STUDENT
            // =================================================

            if (!request.StudentId.HasValue)
            {
                throw new ArgumentException(
                    "StudentId is required.");
            }

            var student =
                await _studentRepository.GetByIdAsync(
                    request.StudentId.Value);

            if (student == null)
            {
                throw new KeyNotFoundException(
                    "Student not found.");
            }

            if (string.IsNullOrWhiteSpace(student.EmailAddress))
            {
                throw new ArgumentException(
                    "Student email address is not available.");
            }

            // =================================================
            // CREATE REGISTRATION ENTITY
            // =================================================

            var registration = new StudentRegistration
            {
                StudentId =
                    request.StudentId,

                RegistrationDate =
                    request.RegistrationDate,

                Discount =
                    request.Discount,

                CourseId =
                    request.CourseId,

                CurrentStatus =
                    request.CurrentStatus
            };

            // =================================================
            // INSERT REGISTRATION
            // =================================================

            var result =
                await _repository.AddAsync(registration);

            // =================================================
            // SEND REGISTRATION EMAIL
            // =================================================
            // Student Code is included for identification.
            // Password is NOT included because it was already
            // sent when the student account was created.

            var emailSubject =
                "Student Registration Successful";

            var emailBody =
                $"Dear {student.StudentName},\n\n" +

                $"Your registration has been completed successfully.\n\n" +

                $"Student Details\n" +
                $"-------------------------\n" +
                $"Student Name: {student.StudentName}\n" +
                $"Student Code: {student.StudentCode}\n\n" +

                $"Registration Details\n" +
                $"-------------------------\n" +
                $"Registration ID: {result.RegistrationId}\n" +
                $"Course: {result.CourseName}\n" +
                $"Registration Date: {result.RegistrationDate:dd-MM-yyyy}\n" +
                $"Discount: {result.Discount}\n" +
                $"Status: {result.CurrentStatus}\n\n" +

                $"Your login credentials were sent separately " +
                $"when your student account was created.\n\n" +

                $"Regards,\n" +
                $"Student Management Team";

            await _emailService.SendEmailAsync(
                student.EmailAddress,
                emailSubject,
                emailBody);

            return MapToResponse(result);
        }

        // =====================================================
        // UPDATE
        // =====================================================
        public async Task UpdateAsync(
            int registrationId,
            StudentRegistrationRequestDto request)
        {
            if (registrationId <= 0)
            {
                throw new ArgumentException(
                    "RegistrationId must be greater than 0.");
            }

            if (request == null)
            {
                throw new ArgumentException(
                    "Registration data is required.");
            }

            ValidateRegistration(request);

            var existing =
                await _repository.GetByIdAsync(
                    registrationId);

            if (existing == null)
            {
                throw new KeyNotFoundException(
                    "Registration not found.");
            }

            existing.StudentId =
                request.StudentId;

            existing.RegistrationDate =
                request.RegistrationDate;

            existing.Discount =
                request.Discount;

            existing.CourseId =
                request.CourseId;

            existing.CurrentStatus =
                request.CurrentStatus;

            await _repository.UpdateAsync(existing);
        }

        // =====================================================
        // DELETE
        // =====================================================
        public async Task DeleteAsync(
            int registrationId)
        {
            if (registrationId <= 0)
            {
                throw new ArgumentException(
                    "RegistrationId must be greater than 0.");
            }

            var existing =
                await _repository.GetByIdAsync(
                    registrationId);

            if (existing == null)
            {
                throw new KeyNotFoundException(
                    "Registration not found.");
            }

            await _repository.DeleteAsync(
                registrationId);
        }

        // =====================================================
        // RESTORE
        // =====================================================
        public async Task RestoreAsync(
            int registrationId)
        {
            if (registrationId <= 0)
            {
                throw new ArgumentException(
                    "RegistrationId must be greater than 0.");
            }

            var existing =
                await _repository.GetByIdAsync(
                    registrationId);

            if (existing == null)
            {
                throw new KeyNotFoundException(
                    "Registration not found.");
            }

            await _repository.RestoreAsync(
                registrationId);
        }

        // =====================================================
        // VALIDATION
        // =====================================================
        private static void ValidateRegistration(
            StudentRegistrationRequestDto request)
        {
            if (!request.StudentId.HasValue ||
                request.StudentId.Value <= 0)
            {
                throw new ArgumentException(
                    "StudentId must be greater than 0.");
            }

            if (request.CourseId <= 0)
            {
                throw new ArgumentException(
                    "CourseId must be greater than 0.");
            }

            if (!request.RegistrationDate.HasValue)
            {
                throw new ArgumentException(
                    "Registration date is required.");
            }

            if (request.Discount.HasValue &&
                request.Discount.Value < 0)
            {
                throw new ArgumentException(
                    "Discount cannot be negative.");
            }

            if (!string.IsNullOrWhiteSpace(
                request.CurrentStatus) &&
                request.CurrentStatus.Length > 20)
            {
                throw new ArgumentException(
                    "Current status cannot exceed 20 characters.");
            }
        }

        // =====================================================
        // ENTITY → RESPONSE DTO
        // =====================================================
        private static StudentRegistrationResponseDto
            MapToResponse(
                StudentRegistration registration)
        {
            return new StudentRegistrationResponseDto
            {
                RegistrationId =
                    registration.RegistrationId,

                StudentId =
                    registration.StudentId,

                StudentName =
                    registration.StudentName,

                RegistrationDate =
                    registration.RegistrationDate,

                Discount =
                    registration.Discount,

                CourseId =
                    registration.CourseId,

                CourseName =
                    registration.CourseName,

                FeesAmount =
                    registration.FeesAmount,

                FeesChangeDate =
                    registration.FeesChangeDate,

                InstallmentPercentage =
                    registration.InstallmentPercentage,

                CurrentStatus =
                    registration.CurrentStatus
            };
        }
    }
}


