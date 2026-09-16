using System;

namespace LeadManagement.Application.DTOs.EnquiryFollowup
{
    public class EnquiryFollowupDto
    {
       
        public string? CandidateName { get; set; }

        public int FollowupId { get; set; }

        public int? EnquiryId { get; set; }

        public int? SourceId { get; set; }

        public string? SourceName { get; set; }

        public DateTime? FollowUpDate { get; set; }

        public string? FollowUpBy { get; set; }

        public string? Description { get; set; }

        public string? Status { get; set; }

        public DateTime? NextFollowupDate { get; set; }
    }
}

