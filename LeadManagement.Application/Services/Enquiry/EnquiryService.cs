using LeadManagement.Application.DTOs.Enquiry;
using LeadManagement.Application.Interfaces.Repositories.Enquiry;
using LeadManagement.Application.Interfaces.Services.Enquiry;

namespace LeadManagement.Application.Services.Enquiry
{
    public class EnquiryService : IEnquiryService
    {
        private readonly IEnquiryRepository _enquiryRepository;

        public EnquiryService(IEnquiryRepository enquiryRepository)
        {
            _enquiryRepository = enquiryRepository;
        }

        public async Task<int> CreateAsync(EnquiryDto enquiry)
        {
            return await _enquiryRepository.CreateAsync(enquiry);
        }

        public async Task<bool> UpdateAsync(EnquiryDto enquiry)
        {
            return await _enquiryRepository.UpdateAsync(enquiry);
        }

        public async Task<EnquiryDto?> GetByIdAsync(int enquiryId)
        {
            return await _enquiryRepository.GetByIdAsync(enquiryId);
        }

        public async Task<IEnumerable<EnquiryDto>> GetAllAsync()
        {
            return await _enquiryRepository.GetAllAsync();
        }

        public async Task<bool> DeleteAsync(int enquiryId)
        {
            return await _enquiryRepository.DeleteAsync(enquiryId);
        }

        public async Task<bool> RestoreAsync(int enquiryId)
        {
            return await _enquiryRepository.RestoreAsync(enquiryId);
        }

        public async Task<IEnumerable<CandidateDropdownDto>> GetCandidatesAsync()
        {
            return await _enquiryRepository.GetCandidatesAsync();
        }
    }
}