using System;

namespace LeadManagement.Domain.Entities
{
    public class TblLeadFollowup
    {
        public int LeadFollowupId { get; set; }

        public int? LeadId { get; set; }

        public DateTime? FollowUpDate { get; set; }

        public string? FollowUpBy { get; set; }

        public string? Description { get; set; }

        public string? Status { get; set; }

        public DateTime? NextFollowupDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public DateTime? RestoredAt { get; set; }

        // For displaying Lead information
        public string? CandidateName { get; set; }
    }
}