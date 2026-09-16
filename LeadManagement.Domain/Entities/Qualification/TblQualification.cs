using System;

namespace LeadManagement.Domain.Entities
{
    public class TblQualification
    {
        public int QualificationId { get; set; }

        public string Qualification { get; set; } = string.Empty;

        public int? Flag { get; set; }

        public DateTime? InsertedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public DateTime? RestoredAt { get; set; }
    }
}