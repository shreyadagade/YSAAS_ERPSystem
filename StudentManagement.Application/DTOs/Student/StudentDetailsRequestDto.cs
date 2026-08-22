namespace StudentManagement.Application.DTOs.Student
{
    public class StudentDetailsRequestDto
    {
        public string StudentName { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

        public string MobileNumber { get; set; } = string.Empty;

        public string EmailAddress { get; set; } = string.Empty;

        public DateTime? BirthDate { get; set; }

        public string? ProfilePhoto { get; set; }

        public string? Qualification { get; set; }

        public string ParentName { get; set; } = string.Empty;

        public string ParentNumber { get; set; } = string.Empty;

       // public string? StudentCode { get; set; }

        public string LastName { get; set; } = string.Empty;

        public string? WhatsappNumber { get; set; }

        public string? LocalAddress { get; set; }

        public string? PermanentAddress { get; set; }

        public string PermanentIdentificationNumber { get; set; } = string.Empty;

        public string? AadharCardNumber { get; set; }

        public string? AadharCardPhoto { get; set; }

        public int? BranchId { get; set; }
    }
}