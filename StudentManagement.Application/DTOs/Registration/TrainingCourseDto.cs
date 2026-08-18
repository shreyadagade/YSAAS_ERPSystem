using System;
using System.Collections.Generic;
using System.Text;

  
 namespace StudentManagement.Application.DTOs
    {
        public class TrainingCourseDto
        {
            public int CourseId { get; set; }

            public string? CourseName { get; set; }

            public double? FeesAmount { get; set; }

            public DateTime? FeesChangeDate { get; set; }

            public double? InstallmentPercentage { get; set; }
        }

        public class TrainingCourseResponseDto
        {
            public int StatusCode { get; set; }

            public string? Message { get; set; }

            public List<TrainingCourseDto> Data { get; set; }
                = new List<TrainingCourseDto>();
        }
    }

