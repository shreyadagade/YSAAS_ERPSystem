using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.Application.DTOs.Qualification
{
  
            public class StudentQualificationResponseDto
        {
            public int QualificationId { get; set; }

            public int? StudentId { get; set; }

            public string? StudentName { get; set; }

            public string? Qualification { get; set; }

            public int? PassingYear { get; set; }

            public string? University { get; set; }

            public string? Medium { get; set; }

            public double? Percentage { get; set; }
        }
    }

