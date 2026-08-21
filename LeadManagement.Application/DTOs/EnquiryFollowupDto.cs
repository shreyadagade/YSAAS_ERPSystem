using System;
using System.Collections.Generic;
using System.Text;

namespace LeadManagement.Application.DTOs
{
    

    public class EnquiryFollowupDto
    {
        public int FollowupId { get; set; }
        public int? EnquiryId { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public string? FollowUpBy { get; set; }
        public string? Description { get; set; }
        //public DateTime CreatedAt { get; set; }
        //public DateTime? UpdatedAt { get; set; }
        //public DateTime? DeletedAt { get; set; }
        //public DateTime? RestoredAt { get; set; }
    }
}

