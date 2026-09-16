using LeadManagement.Application.DTOs.Qualification;

namespace LeadManagement.Application.Interfaces.Services
{
    public interface IQualificationService
    {
        Task<int> CreateAsync(QualificationDto qualification);

        Task<bool> UpdateAsync(QualificationDto qualification);

        Task<bool> DeleteAsync(int qualificationId);

        Task<bool> RestoreAsync(int qualificationId);

        Task<QualificationDto?> GetByIdAsync(int qualificationId);

        Task<IEnumerable<QualificationDto>> GetAllAsync();
    }
}