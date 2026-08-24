
namespace StudentManagement.Domain.Entities.Student
{
    public class StudentQualification
    {
        public int QualificationId { get; set; }

        public int? StudentId { get; set; }

        public string? Qualification { get; set; }

        public int? PassingYear { get; set; }

        public string? University { get; set; }

        public string? Medium { get; set; }

        public double? Percentage { get; set; }

        public int? Flag { get; set; }

        public DateTime? InsertedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public DateTime? RestoredAt { get; set; }
    }
}

