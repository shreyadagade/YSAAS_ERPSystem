using LeadManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeadManagement.Application.Interfaces.Repositories.TrainingCourse
{
   

    public interface ITrainingCourseRepository
    {
        Task<int> InsertAsync(TblTrainingCourse course);

        Task<bool> UpdateAsync(TblTrainingCourse course);

        Task<bool> DeleteAsync(int courseId);

        Task<bool> RestoreAsync(int courseId);

        Task<TblTrainingCourse?> GetByIdAsync(int courseId);

        Task<IEnumerable<TblTrainingCourse>> GetAllAsync();
        Task<bool> CourseNameExistsAsync(string courseName, int? courseId = null);
    }
}

