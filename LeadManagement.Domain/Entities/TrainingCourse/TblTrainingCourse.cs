using System;
using System.Collections.Generic;
using System.Text;
using LeadManagement.Domain.Entities.TrainingCourse;

namespace LeadManagement.Domain.Entities.TrainingCourse
{


    public class TblTrainingCourse
    {
        public int CourseId { get; set; }

        public string CourseName { get; set; } = string.Empty;

        public int? Flag { get; set; }
        public double? FeesAmount { get; set; }

        public DateTime? FeesChangeDate { get; set; }

        public double? InstallmentPercentage { get; set; }

        public DateTime? InsertedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public DateTime? RestoredAt { get; set; }
    }
}

