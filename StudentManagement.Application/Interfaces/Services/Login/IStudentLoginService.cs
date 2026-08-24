using StudentManagement.Application.DTOs.Login;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.Application.Interfaces.Services.Login
{

            public interface IStudentLoginService
        {
            Task<StudentLoginResponseDto> LoginAsync(
                StudentLoginRequestDto request);
        }
    
}