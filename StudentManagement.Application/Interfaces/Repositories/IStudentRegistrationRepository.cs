using StudentManagement.Domain.Entities.Registration;

namespace StudentManagement.Application.Interfaces.Repositories.Registration
{
    public interface IStudentRegistrationRepository
    {
        Task<StudentRegistration?> GetByIdAsync(
            int registrationId);

        Task<IEnumerable<StudentRegistration>> GetAllAsync();

        Task<StudentRegistration> AddAsync(
            StudentRegistration registration);

        Task UpdateAsync(
            StudentRegistration registration);

        Task DeleteAsync(
            int registrationId);

        Task RestoreAsync(
            int registrationId);
    }
}