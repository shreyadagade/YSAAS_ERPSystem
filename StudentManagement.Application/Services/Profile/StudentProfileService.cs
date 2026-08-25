using StudentManagement.Application.DTOs.StudentProfile;
using StudentManagement.Application.Interfaces.Repositories.Profile;
using StudentManagement.Application.Interfaces.Services.StudentProfile;
using System;
using System.Collections.Generic;
using System.Text;


namespace StudentManagement.Application.Services.Profile
    {
        public class StudentProfileService : IStudentProfileService
        {
            private readonly IStudentProfileRepository _profileRepository;

            public StudentProfileService(
                IStudentProfileRepository profileRepository)
            {
                _profileRepository = profileRepository;
            }

            public async Task<StudentProfileDto?> GetMyProfileAsync(
                int studentId)
            {
                if (studentId <= 0)
                {
                    throw new ArgumentException(
                        "Invalid student ID.");
                }

                return await _profileRepository
                    .GetProfileByStudentIdAsync(studentId);
            }
        }
    }

