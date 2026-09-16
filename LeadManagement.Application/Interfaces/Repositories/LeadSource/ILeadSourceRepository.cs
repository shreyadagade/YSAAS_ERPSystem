using LeadManagement.Domain.Entities.LeadSource;

namespace LeadManagement.Application.Interfaces.Repositories.LeadSource
{
    public interface ILeadSourceRepository
    {
        Task<int> InsertAsync(TblLeadSource source);

        Task<bool> UpdateAsync(TblLeadSource source);

        Task<bool> DeleteAsync(int sourceId);

        Task<bool> RestoreAsync(int sourceId);

        Task<TblLeadSource?> GetByIdAsync(int sourceId);

        Task<IEnumerable<TblLeadSource>> GetAllAsync();

        Task<bool> SourceNameExistsAsync(
            string sourceName,
            int? sourceId = null);
    }
}