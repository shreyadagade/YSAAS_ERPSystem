using System;

namespace StudentManagement.Domain.Entities.Course
{
    public class Course
    {
        public int CourseId { get; set; }

        public string CourseName { get; set; } = string.Empty;

        public int? Flag { get; set; }

        public DateTime? InsertedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public DateTime? RestoredAt { get; set; }

        public double? FeesAmount { get; set; }

        public DateTime? FeesChangeDate { get; set; }

        public double? InstallmentPercentage { get; set; }
    }
}