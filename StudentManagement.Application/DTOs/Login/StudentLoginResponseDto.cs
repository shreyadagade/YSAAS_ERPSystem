using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.Application.DTOs.Login

    {
        public class StudentLoginResponseDto
        {
            public int StudentId { get; set; }

            public string? StudentCode { get; set; }

            public string? StudentName { get; set; }

            public string? EmailAddress { get; set; }

            public string Message { get; set; } = string.Empty;
        }
    }
