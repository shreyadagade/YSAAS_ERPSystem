using LeadManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeadManagement.Application.Interfaces.Repositories
{
    

    public interface IEnquiryFollowupRepository
    {
        Task<int> InsertAsync(TblEnquiryFollowup followup);

        Task<bool> UpdateAsync(TblEnquiryFollowup followup);

        Task<bool> DeleteAsync(int followupId);

        Task<bool> RestoreAsync(int followupId);

        Task<TblEnquiryFollowup?> GetByIdAsync(int followupId);

        Task<IEnumerable<TblEnquiryFollowup>> GetAllAsync();
    }
}

