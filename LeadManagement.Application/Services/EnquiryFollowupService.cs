
using LeadManagement.Application.DTOs.EnquiryFollowup;
using LeadManagement.Application.Interfaces.Repositories.EnquiryFollowup;
using LeadManagement.Application.Interfaces.Services;
using LeadManagement.Domain.Entities.EnquiryFollowup;
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

        // =====================================================
        // CREATE
        // =====================================================

        public async Task<int> CreateAsync(
            EnquiryFollowupDto followup)
        {
            _logger.LogInformation(
                "Creating enquiry follow-up. EnquiryId: {EnquiryId}",
                followup.EnquiryId);

            // 1. Follow-up date validation
            if (followup.FollowUpDate == null)
            {
                _logger.LogWarning(
                    "Enquiry follow-up creation failed: Follow-up date is required.");

                throw new ArgumentException(
                    "Follow-up date is required.");
            }

            // 2. Follow-up by validation
            if (string.IsNullOrWhiteSpace(followup.FollowUpBy))
            {
                _logger.LogWarning(
                    "Enquiry follow-up creation failed: Follow-up by is required.");

                throw new ArgumentException(
                    "Follow-up by is required.");
            }

            if (followup.FollowUpBy.Length > 100)
            {
                _logger.LogWarning(
                    "Enquiry follow-up creation failed: Follow-up by exceeds 100 characters.");

                throw new ArgumentException(
                    "Follow-up by cannot exceed 100 characters.");
            }

            // 3. Description validation
            if (!string.IsNullOrWhiteSpace(followup.Description) &&
                followup.Description.Length > 8000)
            {
                _logger.LogWarning(
                    "Enquiry follow-up creation failed: Description exceeds 8000 characters.");

                throw new ArgumentException(
                    "Description cannot exceed 8000 characters.");
            }

            // 4. Status validation
            if (!string.IsNullOrWhiteSpace(followup.Status))
            {
                var status = followup.Status.Trim();

                if (status != "Hot" &&
                    status != "Warm" &&
                    status != "Cold")
                {
                    _logger.LogWarning(
                        "Enquiry follow-up creation failed: Invalid status.");

                    throw new ArgumentException(
                        "Status must be Hot, Warm, or Cold.");
                }
            }

            // 5. Next follow-up date validation
            if (followup.NextFollowupDate != null &&
                followup.NextFollowupDate < followup.FollowUpDate)
            {
                _logger.LogWarning(
                    "Enquiry follow-up creation failed: Next follow-up date cannot be earlier than follow-up date.");

                throw new ArgumentException(
                    "Next follow-up date cannot be earlier than follow-up date.");
            }

            // 6. Source ID validation
            if (followup.SourceId.HasValue &&
                followup.SourceId.Value <= 0)
            {
                _logger.LogWarning(
                    "Enquiry follow-up creation failed: Invalid SourceId.");

                throw new ArgumentException(
                    "Invalid source ID.");
            }

            var entity = new TblEnquiryFollowup
            {
                EnquiryId = followup.EnquiryId,
                SourceId = followup.SourceId,
                FollowUpDate = followup.FollowUpDate,
                FollowUpBy = followup.FollowUpBy.Trim(),
                Description = followup.Description,
                Status = string.IsNullOrWhiteSpace(followup.Status)
                    ? null
                    : followup.Status.Trim(),
                NextFollowupDate = followup.NextFollowupDate
            };

            var followupId =
                await _followupRepository.InsertAsync(entity);

            _logger.LogInformation(
                "Enquiry follow-up created successfully. FollowupId: {FollowupId}",
                followupId);

            return followupId;
        }

        // =====================================================
        // UPDATE
        // =====================================================

        public async Task<bool> UpdateAsync(
            EnquiryFollowupDto followup)
        {
            _logger.LogInformation(
                "Updating enquiry follow-up. FollowupId: {FollowupId}",
                followup.FollowupId);

            // 1. Follow-up ID validation
            if (followup.FollowupId <= 0)
            {
                _logger.LogWarning(
                    "Enquiry follow-up update failed: Invalid FollowupId.");

                throw new ArgumentException(
                    "Invalid follow-up ID.");
            }

            // 2. Follow-up date validation
            if (followup.FollowUpDate == null)
            {
                _logger.LogWarning(
                    "Enquiry follow-up update failed: Follow-up date is required.");

                throw new ArgumentException(
                    "Follow-up date is required.");
            }

            // 3. Follow-up by validation
            if (string.IsNullOrWhiteSpace(followup.FollowUpBy))
            {
                _logger.LogWarning(
                    "Enquiry follow-up update failed: Follow-up by is required.");

                throw new ArgumentException(
                    "Follow-up by is required.");
            }

            if (followup.FollowUpBy.Length > 100)
            {
                _logger.LogWarning(
                    "Enquiry follow-up update failed: Follow-up by exceeds 100 characters.");

                throw new ArgumentException(
                    "Follow-up by cannot exceed 100 characters.");
            }

            // 4. Description validation
            if (!string.IsNullOrWhiteSpace(followup.Description) &&
                followup.Description.Length > 8000)
            {
                _logger.LogWarning(
                    "Enquiry follow-up update failed: Description exceeds 8000 characters.");

                throw new ArgumentException(
                    "Description cannot exceed 8000 characters.");
            }

            // 5. Status validation
            if (!string.IsNullOrWhiteSpace(followup.Status))
            {
                var status = followup.Status.Trim();

                if (status != "Hot" &&
                    status != "Warm" &&
                    status != "Cold")
                {
                    _logger.LogWarning(
                        "Enquiry follow-up update failed: Invalid status.");

                    throw new ArgumentException(
                        "Status must be Hot, Warm, or Cold.");
                }
            }

            // 6. Next follow-up date validation
            if (followup.NextFollowupDate != null &&
                followup.NextFollowupDate < followup.FollowUpDate)
            {
                _logger.LogWarning(
                    "Enquiry follow-up update failed: Next follow-up date cannot be earlier than follow-up date.");

                throw new ArgumentException(
                    "Next follow-up date cannot be earlier than follow-up date.");
            }

            // 7. Source ID validation
            if (followup.SourceId.HasValue &&
                followup.SourceId.Value <= 0)
            {
                _logger.LogWarning(
                    "Enquiry follow-up update failed: Invalid SourceId.");

                throw new ArgumentException(
                    "Invalid source ID.");
            }

            var entity = new TblEnquiryFollowup
            {
                FollowupId = followup.FollowupId,
                EnquiryId = followup.EnquiryId,
                SourceId = followup.SourceId,
                FollowUpDate = followup.FollowUpDate,
                FollowUpBy = followup.FollowUpBy.Trim(),
                Description = followup.Description,
                Status = string.IsNullOrWhiteSpace(followup.Status)
                    ? null
                    : followup.Status.Trim(),
                NextFollowupDate = followup.NextFollowupDate
            };

            var result =
                await _followupRepository.UpdateAsync(entity);

            if (result)
            {
                _logger.LogInformation(
                    "Enquiry follow-up updated successfully. FollowupId: {FollowupId}",
                    followup.FollowupId);
            }
            else
            {
                _logger.LogWarning(
                    "Enquiry follow-up update failed or follow-up not found. FollowupId: {FollowupId}",
                    followup.FollowupId);
            }

            return result;
        }

        // =====================================================
        // DELETE
        // =====================================================

        public async Task<bool> DeleteAsync(int followupId)
        {
            _logger.LogInformation(
                "Deleting enquiry follow-up. FollowupId: {FollowupId}",
                followupId);

            if (followupId <= 0)
            {
                _logger.LogWarning(
                    "Enquiry follow-up deletion failed: Invalid FollowupId.");

                throw new ArgumentException(
                    "Invalid follow-up ID.");
            }

            var result =
                await _followupRepository.DeleteAsync(followupId);

            if (result)
            {
                _logger.LogInformation(
                    "Enquiry follow-up deleted successfully. FollowupId: {FollowupId}",
                    followupId);
            }
            else
            {
                _logger.LogWarning(
                    "Enquiry follow-up delete failed or follow-up not found. FollowupId: {FollowupId}",
                    followupId);
            }

            return result;
        }

        // =====================================================
        // RESTORE
        // =====================================================

        public async Task<bool> RestoreAsync(int followupId)
        {
            _logger.LogInformation(
                "Restoring enquiry follow-up. FollowupId: {FollowupId}",
                followupId);

            if (followupId <= 0)
            {
                _logger.LogWarning(
                    "Enquiry follow-up restore failed: Invalid FollowupId.");

                throw new ArgumentException(
                    "Invalid follow-up ID.");
            }

            var result =
                await _followupRepository.RestoreAsync(followupId);

            if (result)
            {
                _logger.LogInformation(
                    "Enquiry follow-up restored successfully. FollowupId: {FollowupId}",
                    followupId);
            }
            else
            {
                _logger.LogWarning(
                    "Enquiry follow-up restore failed or follow-up not found. FollowupId: {FollowupId}",
                    followupId);
            }

            return result;
        }

        // =====================================================
        // GET BY ID
        // =====================================================

        public async Task<EnquiryFollowupDto?> GetByIdAsync(
            int followupId)
        {
            _logger.LogInformation(
                "Getting enquiry follow-up by ID. FollowupId: {FollowupId}",
                followupId);

            if (followupId <= 0)
            {
                _logger.LogWarning(
                    "Get enquiry follow-up failed: Invalid FollowupId.");

                throw new ArgumentException(
                    "Invalid follow-up ID.");
            }

            var entity =
                await _followupRepository.GetByIdAsync(followupId);

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
                CandidateName = entity.CandidateName,
                EnquiryId = entity.EnquiryId,
                SourceId = entity.SourceId,
                SourceName = entity.SourceName,
                FollowUpDate = entity.FollowUpDate,
                FollowUpBy = entity.FollowUpBy,
                Description = entity.Description,
                Status = entity.Status,
                NextFollowupDate = entity.NextFollowupDate
            };
        }

        // =====================================================
        // GET ALL
        // =====================================================

        public async Task<IEnumerable<EnquiryFollowupDto>> GetAllAsync()
        {
            _logger.LogInformation(
                "Getting all active enquiry follow-ups.");

            var entities =
                await _followupRepository.GetAllAsync();

            var followups = entities.Select(entity =>
                new EnquiryFollowupDto
                {
                    FollowupId = entity.FollowupId,
                    CandidateName = entity.CandidateName,
                    EnquiryId = entity.EnquiryId,
                    SourceId = entity.SourceId,
                    SourceName = entity.SourceName,
                    FollowUpDate = entity.FollowUpDate,
                    FollowUpBy = entity.FollowUpBy,
                    Description = entity.Description,
                    Status = entity.Status,
                    NextFollowupDate = entity.NextFollowupDate
                });

            _logger.LogInformation(
                "Retrieved {Count} enquiry follow-ups.",
                followups.Count());

            return followups;
        }

        // =====================================================
        // GET BY ENQUIRY ID
        // =====================================================

        public async Task<IEnumerable<EnquiryFollowupDto>> GetByEnquiryIdAsync(
            int enquiryId)
        {
            _logger.LogInformation(
                "Getting enquiry follow-ups by EnquiryId: {EnquiryId}",
                enquiryId);

            if (enquiryId <= 0)
            {
                _logger.LogWarning(
                    "Get enquiry follow-ups failed: Invalid EnquiryId.");

                throw new ArgumentException(
                    "Invalid enquiry ID.");
            }

            var entities =
                await _followupRepository.GetByEnquiryIdAsync(enquiryId);

            var followups = entities.Select(entity =>
                new EnquiryFollowupDto
                {
                    FollowupId = entity.FollowupId,
                    CandidateName = entity.CandidateName,
                    EnquiryId = entity.EnquiryId,
                    SourceId = entity.SourceId,
                    SourceName = entity.SourceName,
                    FollowUpDate = entity.FollowUpDate,
                    FollowUpBy = entity.FollowUpBy,
                    Description = entity.Description,
                    Status = entity.Status,
                    NextFollowupDate = entity.NextFollowupDate
                });

            _logger.LogInformation(
                "Retrieved {Count} enquiry follow-ups for EnquiryId: {EnquiryId}",
                followups.Count(),
                enquiryId);

            return followups;
        }
    }
}
