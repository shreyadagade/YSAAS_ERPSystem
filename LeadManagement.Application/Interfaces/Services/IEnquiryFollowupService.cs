using LeadManagement.Application.DTOs.EnquiryFollowup;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeadManagement.Application.Interfaces.Services
{
   

    public interface IEnquiryFollowupService
    {
        Task<int> CreateAsync(EnquiryFollowupDto followup);

        Task<bool> UpdateAsync(EnquiryFollowupDto followup);

        Task<bool> DeleteAsync(int followupId);

        Task<bool> RestoreAsync(int followupId);

        Task<EnquiryFollowupDto?> GetByIdAsync(int followupId);

        Task<IEnumerable<EnquiryFollowupDto>> GetAllAsync();
    }
}

