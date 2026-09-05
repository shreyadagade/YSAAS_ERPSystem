using LeadManagement.Application.DTOs.Enquiry;

namespace LeadManagement.Application.Interfaces.Services.Enquiry
{
    public interface IEnquiryService
    {
        Task<int> CreateAsync(EnquiryDto enquiry);

        Task<bool> UpdateAsync(EnquiryDto enquiry);

        Task<EnquiryDto?> GetByIdAsync(int enquiryId);

        Task<IEnumerable<EnquiryDto>> GetAllAsync();

        Task<bool> DeleteAsync(int enquiryId);

        Task<bool> RestoreAsync(int enquiryId);

        Task<IEnumerable<CandidateDropdownDto>> GetCandidatesAsync();
    }
}