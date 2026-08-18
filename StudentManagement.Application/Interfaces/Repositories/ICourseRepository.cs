using System;
using System.Collections.Generic;
using System.Text;
using StudentManagement.Domain.Entities.Course;

      
namespace StudentManagement.Application.Interfaces.Repositories.Course
    {
        public interface ICourseRepository
        {
            Task<Domain.Entities.Course.Course?> GetCourseByIdAsync(int courseId);
        }
    }

