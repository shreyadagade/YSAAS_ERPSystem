using LeadManagement.Application.DTOs.Qualification;
using LeadManagement.Application.Interfaces.Repositories.Qualification;
using LeadManagement.Application.Interfaces.Services;
using LeadManagement.Domain.Entities;

namespace LeadManagement.Application.Services
{
    public class QualificationService : IQualificationService
    {
        private readonly IQualificationRepository _repository;

        public QualificationService(
            IQualificationRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> CreateAsync(
            QualificationDto qualification)
        {
            if (string.IsNullOrWhiteSpace(qualification.Qualification))
            {
                throw new ArgumentException(
                    "Qualification is required.");
            }

            if (qualification.Qualification.Length > 100)
            {
                throw new ArgumentException(
                    "Qualification cannot exceed 100 characters.");
            }

            var entity = new TblQualification
            {
                Qualification = qualification.Qualification.Trim(),
                Flag = qualification.Flag ?? 0
            };

            return await _repository.InsertAsync(entity);
        }

        public async Task<bool> UpdateAsync(
            QualificationDto qualification)
        {
            if (qualification.QualificationId <= 0)
            {
                throw new ArgumentException(
                    "Invalid qualification ID.");
            }

            if (string.IsNullOrWhiteSpace(qualification.Qualification))
            {
                throw new ArgumentException(
                    "Qualification is required.");
            }

            if (qualification.Qualification.Length > 100)
            {
                throw new ArgumentException(
                    "Qualification cannot exceed 100 characters.");
            }

            var entity = new TblQualification
            {
                QualificationId = qualification.QualificationId,
                Qualification = qualification.Qualification.Trim(),
                Flag = qualification.Flag ?? 0
            };

            return await _repository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(int qualificationId)
        {
            if (qualificationId <= 0)
            {
                throw new ArgumentException(
                    "Invalid qualification ID.");
            }

            return await _repository.DeleteAsync(qualificationId);
        }

        public async Task<bool> RestoreAsync(int qualificationId)
        {
            if (qualificationId <= 0)
            {
                throw new ArgumentException(
                    "Invalid qualification ID.");
            }

            return await _repository.RestoreAsync(qualificationId);
        }

        public async Task<QualificationDto?> GetByIdAsync(
            int qualificationId)
        {
            if (qualificationId <= 0)
            {
                throw new ArgumentException(
                    "Invalid qualification ID.");
            }

            var entity =
                await _repository.GetByIdAsync(qualificationId);

            if (entity == null)
            {
                return null;
            }

            return new QualificationDto
            {
                QualificationId = entity.QualificationId,
                Qualification = entity.Qualification,
                Flag = entity.Flag
            };
        }

        public async Task<IEnumerable<QualificationDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();

            return entities.Select(entity => new QualificationDto
            {
                QualificationId = entity.QualificationId,
                Qualification = entity.Qualification,
                Flag = entity.Flag
            });
        }
    }
}