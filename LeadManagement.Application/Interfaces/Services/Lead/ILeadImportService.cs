using LeadManagement.Application.DTOs.Lead;

namespace LeadManagement.Application.Interfaces.Services
{
    public interface ILeadImportService
    {
        Task<LeadImportResultDto> ImportExcelAsync(
            Stream excelStream,
            string fileName);
    }
}