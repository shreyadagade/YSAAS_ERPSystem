using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.Application.DTOs.Registration
{
           public class StudentRegistrationResponseDto
        {
            public int RegistrationId { get; set; }

            public int? StudentId { get; set; }

            public string? StudentName { get; set; }

            public DateTime? RegistrationDate { get; set; }

            public double? Discount { get; set; }

            public int? CourseId { get; set; }

            public string? CourseName { get; set; }

            public double? FeesAmount { get; set; }

            public DateTime? FeesChangeDate { get; set; }

            public double? InstallmentPercentage { get; set; }

            public string? CurrentStatus { get; set; }
        }
    }

