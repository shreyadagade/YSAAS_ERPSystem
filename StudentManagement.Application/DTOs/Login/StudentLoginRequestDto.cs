using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.Application.DTOs.Login
{
   
        public class StudentLoginRequestDto
        {
        public string StudentCode { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
        }
}
