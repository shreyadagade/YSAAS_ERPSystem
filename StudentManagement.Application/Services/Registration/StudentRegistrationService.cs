
using StudentManagement.Application.DTOs.Registration;
using StudentManagement.Application.Interfaces.Repositories.Registration;
using StudentManagement.Application.Interfaces.Repositories.Student;
using StudentManagement.Application.Interfaces.Services.Registration;
using StudentManagement.Domain.Entities.Registration;

namespace StudentManagement.Application.Services.Registration
{
    public class StudentRegistrationService
        : IStudentRegistrationService
    {
        private readonly IStudentRegistrationRepository _repository;
        private readonly IStudentDetailsRepository _studentRepository;

        public StudentRegistrationService(
            IStudentRegistrationRepository repository,
            IStudentDetailsRepository studentRepository)
        {
            _repository = repository;
            _studentRepository = studentRepository;
        }

        // =====================================================
        // GET BY ID
        // =====================================================

        public async Task<StudentRegistrationResponseDto?> GetByIdAsync(
            int registrationId)
        {
            if (registrationId <= 0)
            {
                throw new ArgumentException(
                    "RegistrationId must be greater than 0.");
            }

            var registration =
                await _repository.GetByIdAsync(
                    registrationId);

            if (registration == null)
            {
                return null;
            }

            return MapToResponseDto(
                registration);
        }

        // =====================================================
        // GET ALL
        // =====================================================

        public async Task<IEnumerable<StudentRegistrationResponseDto>>
            GetAllAsync()
        {
            var registrations =
                await _repository.GetAllAsync();

            return registrations.Select(
                MapToResponseDto);
        }

        // =====================================================
        // ADD
        // =====================================================

        public async Task<StudentRegistrationResponseDto>
            AddAsync(
                StudentRegistrationRequestDto request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(
                    nameof(request));
            }

            // StudentId is nullable (int?)
            // So validate it first.
            if (!request.StudentId.HasValue ||
                request.StudentId.Value <= 0)
            {
                throw new ArgumentException(
                    "StudentId must be greater than 0.");
            }

            // Convert int? to int using .Value
            var studentId =
                request.StudentId.Value;

            var student =
                await _studentRepository.GetByIdAsync(
                    studentId);

            if (student == null)
            {
                throw new KeyNotFoundException(
                    "Student not found.");
            }

            if (string.IsNullOrWhiteSpace(
                student.EmailAddress))
            {
                throw new InvalidOperationException(
                    "Student email is not available.");
            }

            var registration =
                new StudentRegistration
                {
                    StudentId =
                        studentId,

                    RegistrationDate =
                        request.RegistrationDate,

                    Discount =
                        request.Discount,

                    CourseId =
                        request.CourseId,

                    CurrentStatus =
                        request.CurrentStatus
                };

            var createdRegistration =
                await _repository.AddAsync(
                    registration);

            return MapToResponseDto(
                createdRegistration);
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
                throw new ArgumentNullException(
                    nameof(request));
            }

            // StudentId is nullable (int?)
            if (!request.StudentId.HasValue ||
                request.StudentId.Value <= 0)
            {
                throw new ArgumentException(
                    "StudentId must be greater than 0.");
            }

            var existing =
                await _repository.GetByIdAsync(
                    registrationId);

            if (existing == null)
            {
                throw new KeyNotFoundException(
                    "Registration not found.");
            }

            existing.StudentId =
                request.StudentId.Value;

            existing.RegistrationDate =
                request.RegistrationDate;

            existing.Discount =
                request.Discount;

            existing.CourseId =
                request.CourseId;

            existing.CurrentStatus =
                request.CurrentStatus;

            await _repository.UpdateAsync(
                existing);
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

        public async Task<bool> RestoreAsync(
            int registrationId)
        {
            if (registrationId <= 0)
            {
                throw new ArgumentException(
                    "RegistrationId must be greater than 0.");
            }

            var restored =
                await _repository.RestoreAsync(
                    registrationId);

            return restored;
        }

        // =====================================================
        // MAP RESPONSE DTO
        // =====================================================

        private static StudentRegistrationResponseDto
            MapToResponseDto(
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
