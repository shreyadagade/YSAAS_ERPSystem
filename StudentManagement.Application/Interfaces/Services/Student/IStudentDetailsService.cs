using StudentManagement.Application.DTOs.Student;

namespace StudentManagement.Application.Interfaces.Services.Student
{
    public interface IStudentDetailsService
    {
        // Get student by ID
        Task<StudentDetailsResponseDto?> GetByIdAsync(int studentId);

        // Get all students
        Task<IEnumerable<StudentDetailsResponseDto>> GetAllAsync();

        // Create student
        Task<StudentDetailsResponseDto> AddAsync(
            StudentDetailsRequestDto request);

        // Update student
        Task UpdateAsync(
            int studentId,
            StudentDetailsRequestDto request);

        // Soft delete student
        Task DeleteAsync(int studentId);

        // Restore student
        Task RestoreAsync(int studentId);
    }
}