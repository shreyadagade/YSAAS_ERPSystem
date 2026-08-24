namespace StudentManagement.Domain.Entities.Registration
{
    public class StudentRegistration
    {
        public int RegistrationId { get; set; }

        public int? StudentId { get; set; }

        public DateTime? RegistrationDate { get; set; }

        public double? Discount { get; set; }

        public int? CourseId { get; set; }

        public int? Flag { get; set; }

        public string? CurrentStatus { get; set; }

        public DateTime? InsertedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public DateTime? RestoredAt { get; set; }

        // Data returned by JOIN
        public string? StudentName { get; set; }

        public string? CourseName { get; set; }

        public double? FeesAmount { get; set; }

        public DateTime? FeesChangeDate { get; set; }

        public double? InstallmentPercentage { get; set; }
    }
}