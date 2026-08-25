using StudentManagement.Application.DTOs.StudentProfile;
using System;
using System.Collections.Generic;
using System.Text;


namespace StudentManagement.Application.Interfaces.Services.StudentProfile
    {
        public interface IStudentProfileService
        {
            Task<StudentProfileDto?> GetMyProfileAsync(
                int studentId);
        }
    }

