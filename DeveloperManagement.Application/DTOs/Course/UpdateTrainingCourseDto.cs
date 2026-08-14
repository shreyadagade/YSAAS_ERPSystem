using System;
using System.Collections.Generic;
using System.Text;

namespace DeveloperManagement.Application.DTOs.Course
{
    public class UpdateTrainingCourseDto
    {
        public string CourseName { get; set; } = string.Empty;

        public double? FeesAmount { get; set; }

        public DateTime? FeesChangeDate { get; set; }

        public double? InstallmentPercentage { get; set; }
    }
}
