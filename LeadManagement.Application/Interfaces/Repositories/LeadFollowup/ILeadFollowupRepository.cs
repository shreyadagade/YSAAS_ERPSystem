using LeadManagement.Domain.Entities;

namespace LeadManagement.Application.Interfaces.Repositories.LeadFollowup
{
    public interface ILeadFollowupRepository
    {
        Task<int> CreateAsync(TblLeadFollowup followup);

        Task<bool> UpdateAsync(TblLeadFollowup followup);

        Task<bool> DeleteAsync(int followupId);

        Task<bool> RestoreAsync(int followupId);

        Task<TblLeadFollowup?> GetByIdAsync(int followupId);

        Task<IEnumerable<TblLeadFollowup>> GetAllAsync();
    }
}