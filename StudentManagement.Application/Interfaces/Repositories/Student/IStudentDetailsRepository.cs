using StudentManagement.Domain.Entities.Student;

namespace StudentManagement.Application.Interfaces.Repositories.Student
{
    public interface IStudentDetailsRepository
    {
        // Get one student by ID
        Task<StudentDetails?> GetByIdAsync(int studentId);

        // Get all active students
        Task<IEnumerable<StudentDetails>> GetAllAsync();

        // Create student
        Task<StudentDetails> AddAsync(StudentDetails student);

        // Update student
        Task UpdateAsync(StudentDetails student);

        // Soft delete student
        Task DeleteAsync(int studentId);

        // Restore deleted student
        Task RestoreAsync(int studentId);
    }
}