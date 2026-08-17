using DeveloperManagement.Application.DTOs.CourseTopic;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeveloperManagement.Application.Interfaces
{
    public interface ITrainingCourseTopicService
    {
        Task<List<CourseTopicResponseDto>> CreateMultipleAsync(CreateMultipleCourseTopicDto dto);
        Task<List<CourseTopicResponseDto>> GetAllAsync();

        Task<CourseTopicResponseDto> GetByIdAsync(int courseTopicId);

        Task<List<CourseTopicResponseDto>> GetByCourseAsync(int courseId);

        Task<List<CourseTopicResponseDto>> UpdateCourseTopicsAsync(int courseId,UpdateCourseTopicsDto dto);

        Task DeleteAsync(int courseTopicId);

        Task RestoreAsync(int courseTopicId);
    }
    
}
