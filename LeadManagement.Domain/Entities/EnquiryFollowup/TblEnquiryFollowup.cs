using System;

namespace LeadManagement.Domain.Entities.EnquiryFollowup
{
    public class TblEnquiryFollowup
    {
        public int FollowupId { get; set; }

        public int? EnquiryId { get; set; }

        public int? SourceId { get; set; }

        public string? SourceName { get; set; }

        public string? CandidateName { get; set; }

        public DateTime? FollowUpDate { get; set; }

        public string? FollowUpBy { get; set; }

        public string? Description { get; set; }

        public string? Status { get; set; }

        public DateTime? NextFollowupDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public DateTime? RestoredAt { get; set; }
    }
}

