using StudentManagement.Domain.Entities.Student;

namespace StudentManagement.Application.Interfaces.Repositories.Student
{
    public interface IStudentDetailsRepository
    {
        // GET BY ID
        Task<StudentDetails?> GetByIdAsync(
            int studentId);

        // GET ALL
        Task<IEnumerable<StudentDetails>> GetAllAsync();

        // CREATE
        Task<StudentDetails> AddAsync(
            StudentDetails student);

        // UPDATE
        Task UpdateAsync(
            StudentDetails student);

        // DELETE
        Task DeleteAsync(
            int studentId);

        // RESTORE
        Task RestoreAsync(
            int studentId);

        // LOGIN
        Task<StudentDetails?> GetByStudentCodeAsync(
            string studentCode);
    }
}

