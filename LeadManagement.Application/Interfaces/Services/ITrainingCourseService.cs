using LeadManagement.Application.DTOs.TrainingCourse;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeadManagement.Application.Interfaces.Services
{
   

    public interface ITrainingCourseService
    {
        Task<int> CreateAsync(TrainingCourseDto course);

        Task<bool> UpdateAsync(TrainingCourseDto course);

        Task<bool> DeleteAsync(int courseId);

        Task<bool> RestoreAsync(int courseId);

        Task<TrainingCourseDto?> GetByIdAsync(int courseId);

        Task<IEnumerable<TrainingCourseDto>> GetAllAsync();
    }
}

