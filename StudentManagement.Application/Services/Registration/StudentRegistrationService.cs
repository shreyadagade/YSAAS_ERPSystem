using StudentManagement.Application.Interfaces.Repositories.Registration;
using StudentManagement.Application.Interfaces.Services.Registration;
using StudentManagement.Domain.Entities.Registration;

namespace StudentManagement.Application.Services.Registration
{
    public class StudentRegistrationService : IStudentRegistrationService
    {
        private readonly IStudentRegistrationRepository _repository;

        public StudentRegistrationService(
            IStudentRegistrationRepository repository)
        {
            _repository = repository;
        }

        // =====================================================
        // GET BY ID
        // =====================================================
        public async Task<StudentRegistration?> GetByIdAsync(
            int registrationId)
        {
            if (registrationId <= 0)
            {
                throw new ArgumentException(
                    "RegistrationId must be greater than 0.");
            }

            return await _repository.GetByIdAsync(registrationId);
        }

        // =====================================================
        // GET ALL
        // =====================================================
        public async Task<IEnumerable<StudentRegistration>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        // =====================================================
        // CREATE
        // =====================================================
        public async Task<StudentRegistration> AddAsync(
            StudentRegistration registration)
        {
            if (registration == null)
            {
                throw new ArgumentException(
                    "Registration data is required.");
            }

            ValidateRegistration(registration);

            return await _repository.AddAsync(registration);
        }

        // =====================================================
        // UPDATE
        // =====================================================
        public async Task UpdateAsync(
            StudentRegistration registration)
        {
            if (registration == null)
            {
                throw new ArgumentException(
                    "Registration data is required.");
            }

            if (registration.RegistrationId <= 0)
            {
                throw new ArgumentException(
                    "RegistrationId is required.");
            }

            ValidateRegistration(registration);

            var existing =
                await _repository.GetByIdAsync(
                    registration.RegistrationId);

            if (existing == null)
            {
                throw new KeyNotFoundException(
                    "Registration not found.");
            }

            await _repository.UpdateAsync(registration);
        }

        // =====================================================
        // DELETE
        // =====================================================
        public async Task DeleteAsync(int registrationId)
        {
            if (registrationId <= 0)
            {
                throw new ArgumentException(
                    "Invalid RegistrationId.");
            }

            var existing =
                await _repository.GetByIdAsync(registrationId);

            if (existing == null)
            {
                throw new KeyNotFoundException(
                    "Registration not found.");
            }

            await _repository.DeleteAsync(registrationId);
        }

        // =====================================================
        // RESTORE
        // =====================================================
        public async Task RestoreAsync(int registrationId)
        {
            if (registrationId <= 0)
            {
                throw new ArgumentException(
                    "Invalid RegistrationId.");
            }

            var existing =
                await _repository.GetByIdAsync(registrationId);

            if (existing == null)
            {
                throw new KeyNotFoundException(
                    "Registration not found.");
            }

            await _repository.RestoreAsync(registrationId);
        }

        // =====================================================
        // VALIDATION
        // =====================================================
        private static void ValidateRegistration(
            StudentRegistration registration)
        {
            // =================================================
            // StudentId
            // =================================================
            if (!registration.StudentId.HasValue ||
                registration.StudentId.Value <= 0)
            {
                throw new ArgumentException(
                    "StudentId is required and must be greater than 0.");
            }

            // =================================================
            // CourseId
            // =================================================
            if (!registration.CourseId.HasValue ||
                registration.CourseId.Value <= 0)
            {
                throw new ArgumentException(
                    "CourseId is required and must be greater than 0.");
            }

            // =================================================
            // Registration Date
            // =================================================
            if (!registration.RegistrationDate.HasValue)
            {
                throw new ArgumentException(
                    "RegistrationDate is required.");
            }

            // Registration date cannot be future
            if (registration.RegistrationDate.Value > DateTime.Now)
            {
                throw new ArgumentException(
                    "RegistrationDate cannot be in the future.");
            }

            // =================================================
            // Discount
            // =================================================
            // Discount is treated as percentage.
            // Valid range = 0 to 100.
            if (registration.Discount.HasValue)
            {
                if (registration.Discount.Value < 0 ||
                    registration.Discount.Value > 100)
                {
                    throw new ArgumentException(
                        "Discount must be between 0 and 100.");
                }
            }

            // =================================================
            // Current Status
            // =================================================
            if (string.IsNullOrWhiteSpace(
                registration.CurrentStatus))
            {
                throw new ArgumentException(
                    "CurrentStatus is required.");
            }

            var validStatuses = new[]
            {
                "Active",
                "Completed",
                "Cancelled",
                "OnHold"
            };

            if (!validStatuses.Contains(
                registration.CurrentStatus,
                StringComparer.OrdinalIgnoreCase))
            {
                throw new ArgumentException(
                    "CurrentStatus must be Active, Completed, Cancelled, or OnHold.");
            }

            // =================================================
            // Current Status Length
            // =================================================
            if (registration.CurrentStatus.Length > 20)
            {
                throw new ArgumentException(
                    "CurrentStatus cannot exceed 20 characters.");
            }
        }
    }
}