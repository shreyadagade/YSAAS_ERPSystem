
using LeadManagement.Domain.Entities.EnquiryFollowup;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeadManagement.Application.Interfaces.Repositories.EnquiryFollowup
{
    public interface IEnquiryFollowupRepository
    {
        Task<int> InsertAsync(TblEnquiryFollowup followup);

        Task<bool> UpdateAsync(TblEnquiryFollowup followup);

        Task<bool> DeleteAsync(int followupId);

        Task<bool> RestoreAsync(int followupId);

        Task<TblEnquiryFollowup?> GetByIdAsync(int followupId);

        Task<IEnumerable<TblEnquiryFollowup>> GetAllAsync();

        // Get all follow-ups for a specific enquiry
        Task<IEnumerable<TblEnquiryFollowup>> GetByEnquiryIdAsync(int enquiryId);
    }
}

