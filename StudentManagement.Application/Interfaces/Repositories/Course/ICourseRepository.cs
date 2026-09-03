using System;
using System.Collections.Generic;
using System.Text;

    namespace StudentManagement.Application.Interfaces.Repositories.Course
    {
        public interface ICourseRepository
        {
            Task<bool> CourseExistsAsync(int courseId);
        }
    }

