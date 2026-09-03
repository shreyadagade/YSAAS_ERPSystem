using LeadManagement.Application.DTOs.EnquiryFollowup;
using LeadManagement.Application.Interfaces.Repositories.EnquiryFollowup;
using LeadManagement.Application.Interfaces.Services;
using LeadManagement.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace LeadManagement.Application.Services
{
    public class EnquiryFollowupService : IEnquiryFollowupService
    {
        private readonly IEnquiryFollowupRepository _followupRepository;
        private readonly ILogger<EnquiryFollowupService> _logger;

        public EnquiryFollowupService(
            IEnquiryFollowupRepository followupRepository,
            ILogger<EnquiryFollowupService> logger)
        {
            _followupRepository = followupRepository;
            _logger = logger;
        }

        public async Task<int> CreateAsync(EnquiryFollowupDto followup)
        {
            _logger.LogInformation(
                "Creating enquiry follow-up. EnquiryId: {EnquiryId}",
                followup.EnquiryId);

            // 1. Enquiry ID validation
            if (followup.EnquiryId <= 0)
            {
                _logger.LogWarning(
                    "Follow-up creation failed: Invalid EnquiryId.");

                throw new ArgumentException("Invalid enquiry ID.");
            }

            // 2. Follow-up date validation
            if (followup.FollowUpDate == null)
            {
                _logger.LogWarning(
                    "Follow-up creation failed: Follow-up date is required.");

                throw new ArgumentException("Follow-up date is required.");
            }

            // 3. Follow-up by validation
            if (string.IsNullOrWhiteSpace(followup.FollowUpBy))
            {
                _logger.LogWarning(
                    "Follow-up creation failed: Follow-up by is required.");

                throw new ArgumentException("Follow-up by is required.");
            }

            if (followup.FollowUpBy.Length > 100)
            {
                _logger.LogWarning(
                    "Follow-up creation failed: Follow-up by exceeds 100 characters.");

                throw new ArgumentException(
                    "Follow-up by cannot exceed 100 characters.");
            }

            // 4. Description validation
            if (!string.IsNullOrWhiteSpace(followup.Description) &&
                followup.Description.Length > 8000)
            {
                _logger.LogWarning(
                    "Follow-up creation failed: Description exceeds 8000 characters.");

                throw new ArgumentException(
                    "Description cannot exceed 8000 characters.");
            }

            var entity = new TblEnquiryFollowup
            {
                EnquiryId = followup.EnquiryId,
                FollowUpDate = followup.FollowUpDate,
                FollowUpBy = followup.FollowUpBy.Trim(),
                Description = followup.Description
            };

            var followupId = await _followupRepository.InsertAsync(entity);

            _logger.LogInformation(
                "Enquiry follow-up created successfully. FollowupId: {FollowupId}, EnquiryId: {EnquiryId}",
                followupId,
                followup.EnquiryId);

            return followupId;
        }

        public async Task<bool> UpdateAsync(EnquiryFollowupDto followup)
        {
            _logger.LogInformation(
                "Updating enquiry follow-up. FollowupId: {FollowupId}",
                followup.FollowupId);

            // 1. Follow-up ID validation
            if (followup.FollowupId <= 0)
            {
                _logger.LogWarning(
                    "Follow-up update failed: Invalid FollowupId.");

                throw new ArgumentException("Invalid follow-up ID.");
            }

            // 2. Enquiry ID validation
            if (followup.EnquiryId <= 0)
                throw new ArgumentException("Invalid enquiry ID.");

            // 3. Follow-up date validation
            if (followup.FollowUpDate == null)
                throw new ArgumentException("Follow-up date is required.");

            // 4. Follow-up by validation
            if (string.IsNullOrWhiteSpace(followup.FollowUpBy))
                throw new ArgumentException("Follow-up by is required.");

            if (followup.FollowUpBy.Length > 100)
                throw new ArgumentException(
                    "Follow-up by cannot exceed 100 characters.");

            // 5. Description validation
            if (!string.IsNullOrWhiteSpace(followup.Description) &&
                followup.Description.Length > 8000)
            {
                throw new ArgumentException(
                    "Description cannot exceed 8000 characters.");
            }

            var entity = new TblEnquiryFollowup
            {
                FollowupId = followup.FollowupId,
                EnquiryId = followup.EnquiryId,
                FollowUpDate = followup.FollowUpDate,
                FollowUpBy = followup.FollowUpBy.Trim(),
                Description = followup.Description
            };

            var result = await _followupRepository.UpdateAsync(entity);

            if (result)
            {
                _logger.LogInformation(
                    "Enquiry follow-up updated successfully. FollowupId: {FollowupId}",
                    followup.FollowupId);
            }
            else
            {
                _logger.LogWarning(
                    "Follow-up update failed or follow-up not found. FollowupId: {FollowupId}",
                    followup.FollowupId);
            }

            return result;
        }

        public async Task<bool> DeleteAsync(int followupId)
        {
            _logger.LogInformation(
                "Deleting enquiry follow-up. FollowupId: {FollowupId}",
                followupId);

            if (followupId <= 0)
                throw new ArgumentException("Invalid follow-up ID.");

            var result = await _followupRepository.DeleteAsync(followupId);

            if (result)
            {
                _logger.LogInformation(
                    "Enquiry follow-up deleted successfully. FollowupId: {FollowupId}",
                    followupId);
            }
            else
            {
                _logger.LogWarning(
                    "Follow-up delete failed or follow-up not found. FollowupId: {FollowupId}",
                    followupId);
            }

            return result;
        }

        public async Task<bool> RestoreAsync(int followupId)
        {
            _logger.LogInformation(
                "Restoring enquiry follow-up. FollowupId: {FollowupId}",
                followupId);

            if (followupId <= 0)
                throw new ArgumentException("Invalid follow-up ID.");

            var result = await _followupRepository.RestoreAsync(followupId);

            if (result)
            {
                _logger.LogInformation(
                    "Enquiry follow-up restored successfully. FollowupId: {FollowupId}",
                    followupId);
            }
            else
            {
                _logger.LogWarning(
                    "Follow-up restore failed or follow-up not found. FollowupId: {FollowupId}",
                    followupId);
            }

            return result;
        }

        public async Task<EnquiryFollowupDto?> GetByIdAsync(int followupId)
        {
            _logger.LogInformation(
                "Getting enquiry follow-up by ID. FollowupId: {FollowupId}",
                followupId);

            if (followupId <= 0)
                throw new ArgumentException("Invalid follow-up ID.");

            var entity = await _followupRepository.GetByIdAsync(followupId);

            if (entity == null)
            {
                _logger.LogWarning(
                    "Enquiry follow-up not found. FollowupId: {FollowupId}",
                    followupId);

                return null;
            }

            return new EnquiryFollowupDto
            {
                FollowupId = entity.FollowupId,
                EnquiryId = entity.EnquiryId,
                FollowUpDate = entity.FollowUpDate,
                FollowUpBy = entity.FollowUpBy,
                Description = entity.Description,
                //CreatedAt = entity.CreatedAt,
                //UpdatedAt = entity.UpdatedAt,
                //DeletedAt = entity.DeletedAt,
                //RestoredAt = entity.RestoredAt
            };
        }

        public async Task<IEnumerable<EnquiryFollowupDto>> GetAllAsync()
        {
            _logger.LogInformation(
                "Getting all active enquiry follow-ups.");

            var entities = await _followupRepository.GetAllAsync();

            _logger.LogInformation(
                "Retrieved {Count} enquiry follow-ups.",
                entities.Count());

            return entities.Select(entity => new EnquiryFollowupDto
            {
                FollowupId = entity.FollowupId,
                EnquiryId = entity.EnquiryId,
                FollowUpDate = entity.FollowUpDate,
                FollowUpBy = entity.FollowUpBy,
                Description = entity.Description,
                //CreatedAt = entity.CreatedAt,
                //UpdatedAt = entity.UpdatedAt,
                //DeletedAt = entity.DeletedAt,
                //RestoredAt = entity.RestoredAt
            });
        }
    }
}
