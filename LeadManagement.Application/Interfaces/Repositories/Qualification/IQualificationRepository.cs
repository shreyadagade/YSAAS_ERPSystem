using LeadManagement.Domain.Entities;

namespace LeadManagement.Application.Interfaces.Repositories.Qualification
{
    public interface IQualificationRepository
    {
        Task<int> InsertAsync(TblQualification qualification);

        Task<bool> UpdateAsync(TblQualification qualification);

        Task<bool> DeleteAsync(int qualificationId);

        Task<bool> RestoreAsync(int qualificationId);

        Task<TblQualification?> GetByIdAsync(int qualificationId);

        Task<IEnumerable<TblQualification>> GetAllAsync();
    }
}