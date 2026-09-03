using System;
using System.Collections.Generic;
using System.Text;

namespace LeadManagement.Application.DTOs.Lead
{
  

    public class LeadDto
    {
        public int LeadId { get; set; }
        public string? CandidateName { get; set; }
        public string? EmailAddress { get; set; }
        public string? MobileNumber { get; set; }
        public string? TrainingType { get; set; }
        public string? Description { get; set; }
        public DateTime? LeadDate { get; set; }
        //public DateTime CreatedAt { get; set; }
        //public DateTime? UpdatedAt { get; set; }
        //public DateTime? DeletedAt { get; set; }
        //public DateTime? RestoredAt { get; set; }
    }
}

