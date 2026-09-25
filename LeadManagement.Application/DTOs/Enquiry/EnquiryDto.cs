using System;

namespace LeadManagement.Application.DTOs.Enquiry
{
    public class EnquiryDto
    {
        public int EnquiryId { get; set; }

        //public int? LeadId { get; set; }

        public DateTime? EnquiryDate { get; set; }

        public string? CandidateName { get; set; }

        public string? Gender { get; set; }

        public string? LocalAddress { get; set; }

        public string? EmailAddress { get; set; }

        public string? MobileNumber { get; set; }

        public DateTime? BirthDate { get; set; }
        public int? QualificationID { get; set; }

        public string? Qualification { get; set; }
        public int? SourceId { get; set; }

        public string? LeadSources { get; set; }

        public string? EnquiryFors { get; set; }

        public string? InterestedTopics { get; set; }

        public string? Status { get; set; }

        public int? BranchId { get; set; }

        public string? BranchName { get; set; }
    }
}