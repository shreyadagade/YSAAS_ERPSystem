using StudentManagement.Domain.Entities.Registration;

namespace StudentManagement.Application.Interfaces.Services.Registration
{
    public interface IStudentRegistrationService
    {
        Task<StudentRegistration?> GetByIdAsync(int registrationId);

        Task<IEnumerable<StudentRegistration>> GetAllAsync();

        Task<StudentRegistration> AddAsync(
            StudentRegistration registration);

        Task UpdateAsync(
            StudentRegistration registration);

        Task DeleteAsync(int registrationId);
    }
}