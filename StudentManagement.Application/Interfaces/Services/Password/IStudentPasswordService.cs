using StudentManagement.Application.DTOs.Password;
using System;
using System.Collections.Generic;
using System.Text;


namespace StudentManagement.Application.Interfaces.Services.Password
    {
        public interface IStudentPasswordService
        {
            Task ChangePasswordAsync(
                int studentId,
                ChangePasswordRequestDto request);
        }
    }
