using StudentManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;



namespace StudentManagement.Application.Interfaces.Services
    {
        public interface ITrainingCourseApiClient
        {
            Task<TrainingCourseDto?> GetCourseByIdAsync(int courseId);
        }
    }
