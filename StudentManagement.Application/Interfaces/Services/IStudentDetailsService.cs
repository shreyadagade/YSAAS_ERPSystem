using StudentManagement.Domain.Entities.Registration;

namespace StudentManagement.Application.Interfaces.Services.Registration
{
    public interface IStudentDetailsService
    {
        Task<StudentDetails?> GetByIdAsync(int studentId);

        Task<IEnumerable<StudentDetails>> GetAllAsync();

        Task<StudentDetails> AddAsync(StudentDetails student);

        Task UpdateAsync(StudentDetails student);

        Task DeleteAsync(int studentId);

        Task RestoreAsync(int studentId);
    }
}