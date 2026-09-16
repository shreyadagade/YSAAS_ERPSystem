using LeadManagement.Application.DTOs.LeadSource;

namespace LeadManagement.Application.Interfaces.Services
{
    public interface ILeadSourceService
    {
        Task<int> CreateAsync(LeadSourceDto source);

        Task<bool> UpdateAsync(LeadSourceDto source);

        Task<bool> DeleteAsync(int sourceId);

        Task<bool> RestoreAsync(int sourceId);

        Task<LeadSourceDto?> GetByIdAsync(int sourceId);

        Task<IEnumerable<LeadSourceDto>> GetAllAsync();
    }
}