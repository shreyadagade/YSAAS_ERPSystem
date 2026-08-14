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

        public async Task<StudentRegistration?> GetByIdAsync(
            int registrationId)
        {
            return await _repository.GetByIdAsync(registrationId);
        }

        public async Task<IEnumerable<StudentRegistration>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<StudentRegistration> AddAsync(
            StudentRegistration registration)
        {
            ValidateRegistration(registration);

            return await _repository.AddAsync(registration);
        }

        public async Task UpdateAsync(
            StudentRegistration registration)
        {
            if (registration.RegistrationId <= 0)
            {
                throw new ArgumentException(
                    "RegistrationId is required.");
            }

            ValidateRegistration(registration);

            await _repository.UpdateAsync(registration);
        }

        public async Task DeleteAsync(int registrationId)
        {
            if (registrationId <= 0)
            {
                throw new ArgumentException(
                    "Invalid RegistrationId.");
            }

            await _repository.DeleteAsync(registrationId);
        }

        public async Task RestoreAsync(int registrationId)
        {
            if (registrationId <= 0)
            {
                throw new ArgumentException(
                    "Invalid RegistrationId.");
            }

            await _repository.RestoreAsync(registrationId);
        }

        private static void ValidateRegistration(
            StudentRegistration registration)
        {
            // StudentId
            if (!registration.StudentId.HasValue ||
                registration.StudentId <= 0)
            {
                throw new ArgumentException(
                    "StudentId is required.");
            }

            // CourseId
            if (!registration.CourseId.HasValue ||
                registration.CourseId <= 0)
            {
                throw new ArgumentException(
                    "CourseId is required.");
            }

            // Registration Date
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

            // Discount
            if (registration.Discount.HasValue)
            {
                if (registration.Discount < 0)
                {
                    throw new ArgumentException(
                        "Discount cannot be negative.");
                }
            }

            // Current Status
            if (string.IsNullOrWhiteSpace(
                registration.CurrentStatus))
            {
                throw new ArgumentException(
                    "CurrentStatus is required.");
            }

            if (registration.CurrentStatus.Length > 20)
            {
                throw new ArgumentException(
                    "CurrentStatus cannot exceed 20 characters.");
            }
        }
    }
}