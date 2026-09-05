using System;
using System.Collections.Generic;
using System.Text;

namespace LeadManagement.Application.DTOs.Enquiry
{
    public class CandidateDropdownDto
    {
        public int EnquiryId { get; set; }

        public string CandidateName { get; set; } = string.Empty;
    }
}