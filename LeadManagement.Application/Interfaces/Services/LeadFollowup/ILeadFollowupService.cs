using LeadManagement.Application.DTOs.LeadFollowup;

namespace LeadManagement.Application.Interfaces.Services
{
    public interface ILeadFollowupService
    {
        Task<int> CreateAsync(LeadFollowupDto followup);

        Task<bool> UpdateAsync(LeadFollowupDto followup);

        Task<bool> DeleteAsync(int followupId);

        Task<bool> RestoreAsync(int followupId);

        Task<LeadFollowupDto?> GetByIdAsync(int followupId);

        Task<IEnumerable<LeadFollowupDto>> GetAllAsync();
    }
}