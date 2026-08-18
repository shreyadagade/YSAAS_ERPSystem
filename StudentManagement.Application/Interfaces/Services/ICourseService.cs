using System;
using System.Collections.Generic;
using System.Text;



namespace StudentManagement.Application.Interfaces.Services.Course
    {
        public interface ICourseService
        {
            Task<Domain.Entities.Course.Course?> GetCourseByIdAsync(int courseId);
        }
    }

