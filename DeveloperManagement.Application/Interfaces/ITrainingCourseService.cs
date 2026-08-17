using DeveloperManagement.Application.DTOs.Course;
using DeveloperManagement.Application.DTOs.Details;

namespace DeveloperManagement.Application.Interfaces
{
    public interface ITrainingCourseService
    {
        Task<List<TrainingCourseResponseDto>> GetAllAsync();

        Task<TrainingCourseResponseDto> GetByIdAsync(int courseId);

        Task<TrainingCourseResponseDto> CreateAsync(CreateTrainingCourseDto dto);

        Task<TrainingCourseResponseDto> UpdateAsync(int courseId,UpdateTrainingCourseDto dto);

        Task<int> DeleteAsync(int courseId);

        Task<int> RestoreAsync(int courseId);

        Task<CourseDetailsResponseDto> GetCourseDetailsAsync(int courseId);

    }
}