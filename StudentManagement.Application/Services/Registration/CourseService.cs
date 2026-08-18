using StudentManagement.Application.Interfaces.Repositories.Course;
using StudentManagement.Application.Interfaces.Services.Course;
using System;
using System.Collections.Generic;
using System.Text;



namespace StudentManagement.Application.Services.Course
    {
        public class CourseService : ICourseService
        {
            private readonly ICourseRepository _courseRepository;

            public CourseService(ICourseRepository courseRepository)
            {
                _courseRepository = courseRepository;
            }

            public async Task<Domain.Entities.Course.Course?> GetCourseByIdAsync(int courseId)
            {
                return await _courseRepository.GetCourseByIdAsync(courseId);
            }
        }
    }

