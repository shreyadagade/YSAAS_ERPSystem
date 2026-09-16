using LeadManagement.Application.DTOs.Lead;

namespace LeadManagement.Application.Interfaces.Services.Lead
{
    public interface ILeadService
    {
        Task<int> CreateAsync(LeadDto lead);

        Task<bool> UpdateAsync(LeadDto lead);

        Task<bool> DeleteAsync(int leadId);

        Task<bool> RestoreAsync(int leadId);

        Task<LeadDto?> GetByIdAsync(int leadId);

        Task<IEnumerable<LeadDto>> GetAllAsync();

        Task<IEnumerable<LeadDto>> GetBySourceIdAsync(int sourceId);
    }
}