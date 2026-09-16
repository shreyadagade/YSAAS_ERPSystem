using LeadManagement.Application.DTOs.Lead;

namespace LeadManagement.Application.Interfaces.Repositories
{
    public interface ILeadImportRepository
    {
        Task<Dictionary<string, int>> GetActiveSourcesAsync();

        Task<HashSet<string>> GetExistingEmailsAsync(
            IEnumerable<string> emails);

        Task<HashSet<string>> GetExistingMobilesAsync(
            IEnumerable<string> mobiles);

        Task BulkInsertAsync(
            IEnumerable<LeadExcelDto> leads,
            Dictionary<string, int> sourceMap);
    }
}