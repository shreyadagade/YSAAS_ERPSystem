using System;
using System.Collections.Generic;
using System.Text;

namespace DeveloperManagement.Domain.Entities
{
    public class TrainingCourse
    {
        public int CourseId { get; set; }

        public string CourseName { get; set; } = string.Empty;

        public double? FeesAmount { get; set; }

        public DateTime? FeesChangeDate { get; set; }

        public double? InstallmentPercentage { get; set; }
    }
}
