namespace StudentManagement.Domain.Entities.Registration
{
    public class StudentQualification
    {
        public int QualificationId { get; set; }

        public int StudentId { get; set; }

        public string? Qualification { get; set; }

        public int? PassingYear { get; set; }

        public string? University { get; set; }

        public string? Medium { get; set; }

        public double? Percentage { get; set; }
    }
}