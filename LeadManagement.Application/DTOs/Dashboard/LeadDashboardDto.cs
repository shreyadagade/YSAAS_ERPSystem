using System;
using System.Collections.Generic;
using System.Text;

namespace LeadManagement.Application.DTOs.Dashboard
{
    public class LeadDashboardDto
    {
        public int TotalLeads { get; set; }

        public int PaidAdmissions { get; set; }

        public decimal ConversionPercentage { get; set; }
    }
}
