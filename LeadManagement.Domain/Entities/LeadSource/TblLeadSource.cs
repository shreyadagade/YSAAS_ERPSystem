using System;

namespace LeadManagement.Domain.Entities.LeadSource
{
    public class TblLeadSource
    {
        public int SourceId { get; set; }

        public string? SourceName { get; set; }

        public int? Flag { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public DateTime? RestoredAt { get; set; }
    }
}