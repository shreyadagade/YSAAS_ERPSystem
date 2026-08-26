namespace StudentManagement.Application.DTOs.Profile
{
    public class ChangeProfileRequestDto
    {
        public string? StudentName { get; set; }

        public string? LastName { get; set; }

        public string? Gender { get; set; }

        public string? MobileNumber { get; set; }

        public string? WhatsappNumber { get; set; }

        public string? EmailAddress { get; set; }

        public DateTime? BirthDate { get; set; }

        public string? Qualification { get; set; }

        public string? ParentName { get; set; }

        public string? ParentNumber { get; set; }

        public string? LocalAddress { get; set; }

        public string? PermanentAddress { get; set; }

        public string? PermanentIdentificationNumber { get; set; }

        public string? AadharCardNumber { get; set; }

        public string? AadharCardPhoto { get; set; }

        public int? BranchId { get; set; }
    }
}