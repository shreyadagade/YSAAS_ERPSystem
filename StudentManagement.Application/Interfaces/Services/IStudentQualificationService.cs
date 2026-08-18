using StudentManagement.Domain.Entities.Registration;

namespace StudentManagement.Application.Interfaces.Services.Registration
{
    public interface IStudentQualificationService
    {
        Task<StudentQualification?> GetByIdAsync(int qualificationId);

        Task<IEnumerable<StudentQualification>> GetAllAsync();

        Task<StudentQualification> AddAsync(
            StudentQualification qualification);

        Task UpdateAsync(
            StudentQualification qualification);

        Task DeleteAsync(int qualificationId);

        Task RestoreAsync(int qualificationId);
    }
}