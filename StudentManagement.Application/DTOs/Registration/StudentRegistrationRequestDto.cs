using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.Application.DTOs.Registration
{
        public class StudentRegistrationRequestDto
        {
            public int? StudentId { get; set; }

            public DateTime? RegistrationDate { get; set; }

            public double? Discount { get; set; }

            public int? CourseId { get; set; }

            public string? CurrentStatus { get; set; }
        }
    }

