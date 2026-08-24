using StudentManagement.Domain.Entities.Student;

namespace StudentManagement.Application.Interfaces.Repositories.Student
{
    public interface IStudentLoginRepository
    {
        Task<StudentDetails?> GetByStudentCodeAsync(
            string studentCode);
    }
}

