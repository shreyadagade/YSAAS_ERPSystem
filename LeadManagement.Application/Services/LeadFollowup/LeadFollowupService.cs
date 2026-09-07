using LeadManagement.Application.DTOs.LeadFollowup;
using LeadManagement.Application.Interfaces.Repositories.LeadFollowup;
using LeadManagement.Application.Interfaces.Services;
using LeadManagement.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace LeadManagement.Application.Services
{
    public class LeadFollowupService : ILeadFollowupService
    {
        private readonly ILeadFollowupRepository _followupRepository;
        private readonly ILogger<LeadFollowupService> _logger;

        public LeadFollowupService(
            ILeadFollowupRepository followupRepository,
            ILogger<LeadFollowupService> logger)
        {
            _followupRepository = followupRepository;
            _logger = logger;
        }

        // =====================================================
        // CREATE
        // =====================================================

        public async Task<int> CreateAsync(LeadFollowupDto followup)
        {
            _logger.LogInformation(
                "Creating lead follow-up. LeadId: {LeadId}",
                followup.LeadId);

            // 1. Lead ID validation
            if (followup.LeadId <= 0)
            {
                _logger.LogWarning(
                    "Lead follow-up creation failed: Invalid LeadId.");

                throw new ArgumentException(
                    "Invalid lead ID.");
            }

            // 2. Follow-up date validation
            if (followup.FollowUpDate == null)
            {
                _logger.LogWarning(
                    "Lead follow-up creation failed: Follow-up date is required.");

                throw new ArgumentException(
                    "Follow-up date is required.");
            }

            // 3. Follow-up by validation
            if (string.IsNullOrWhiteSpace(followup.FollowUpBy))
            {
                _logger.LogWarning(
                    "Lead follow-up creation failed: Follow-up by is required.");

                throw new ArgumentException(
                    "Follow-up by is required.");
            }

            if (followup.FollowUpBy.Length > 100)
            {
                _logger.LogWarning(
                    "Lead follow-up creation failed: Follow-up by exceeds 100 characters.");

                throw new ArgumentException(
                    "Follow-up by cannot exceed 100 characters.");
            }

            // 4. Description validation
            if (!string.IsNullOrWhiteSpace(followup.Description) &&
                followup.Description.Length > 8000)
            {
                _logger.LogWarning(
                    "Lead follow-up creation failed: Description exceeds 8000 characters.");

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
                        "Lead follow-up creation failed: Invalid status.");

                    throw new ArgumentException(
                        "Status must be Hot, Warm, or Cold.");
                }
            }

            // 6. Next follow-up date validation
            if (followup.NextFollowupDate != null &&
                followup.NextFollowupDate < followup.FollowUpDate)
            {
                _logger.LogWarning(
                    "Lead follow-up creation failed: Next follow-up date cannot be earlier than follow-up date.");

                throw new ArgumentException(
                    "Next follow-up date cannot be earlier than follow-up date.");
            }

            var entity = new TblLeadFollowup
            {
                LeadId = followup.LeadId,
                FollowUpDate = followup.FollowUpDate,
                FollowUpBy = followup.FollowUpBy.Trim(),
                Description = followup.Description,
                Status = string.IsNullOrWhiteSpace(followup.Status)
                    ? null
                    : followup.Status.Trim(),
                NextFollowupDate = followup.NextFollowupDate
            };
            var followupId =
    await _followupRepository.CreateAsync(entity);

            _logger.LogInformation(
                "Lead follow-up created successfully. FollowupId: {FollowupId}, LeadId: {LeadId}",
                followupId,
                followup.LeadId);

            return followupId;
        }

        // =====================================================
        // UPDATE
        // =====================================================

        public async Task<bool> UpdateAsync(
            LeadFollowupDto followup)
        {
            _logger.LogInformation(
                "Updating lead follow-up. LeadFollowupId: {LeadFollowupId}",
                followup.LeadFollowupId);

            // 1. Follow-up ID validation
            if (followup.LeadFollowupId <= 0)
            {
                _logger.LogWarning(
                    "Lead follow-up update failed: Invalid FollowupId.");

                throw new ArgumentException(
                    "Invalid follow-up ID.");
            }

            // 2. Lead ID validation
            if (followup.LeadId <= 0)
            {
                _logger.LogWarning(
                    "Lead follow-up update failed: Invalid LeadId.");

                throw new ArgumentException(
                    "Invalid lead ID.");
            }

            // 3. Follow-up date validation
            if (followup.FollowUpDate == null)
            {
                _logger.LogWarning(
                    "Lead follow-up update failed: Follow-up date is required.");

                throw new ArgumentException(
                    "Follow-up date is required.");
            }

            // 4. Follow-up by validation
            if (string.IsNullOrWhiteSpace(followup.FollowUpBy))
            {
                _logger.LogWarning(
                    "Lead follow-up update failed: Follow-up by is required.");

                throw new ArgumentException(
                    "Follow-up by is required.");
            }

            if (followup.FollowUpBy.Length > 100)
            {
                _logger.LogWarning(
                    "Lead follow-up update failed: Follow-up by exceeds 100 characters.");

                throw new ArgumentException(
                    "Follow-up by cannot exceed 100 characters.");
            }

            // 5. Description validation
            if (!string.IsNullOrWhiteSpace(followup.Description) &&
                followup.Description.Length > 8000)
            {
                _logger.LogWarning(
                    "Lead follow-up update failed: Description exceeds 8000 characters.");

                throw new ArgumentException(
                    "Description cannot exceed 8000 characters.");
            }

            // 6. Status validation
            if (!string.IsNullOrWhiteSpace(followup.Status))
            {
                var status = followup.Status.Trim();

                if (status != "Hot" &&
                    status != "Warm" &&
                    status != "Cold")
                {
                    _logger.LogWarning(
                        "Lead follow-up update failed: Invalid status.");

                    throw new ArgumentException(
                        "Status must be Hot, Warm, or Cold.");
                }
            }

            // 7. Next follow-up date validation
            if (followup.NextFollowupDate != null &&
                followup.NextFollowupDate < followup.FollowUpDate)
            {
                _logger.LogWarning(
                    "Lead follow-up update failed: Next follow-up date cannot be earlier than follow-up date.");

                throw new ArgumentException(
                    "Next follow-up date cannot be earlier than follow-up date.");
            }

            var entity = new TblLeadFollowup
            {
                LeadFollowupId = followup.LeadFollowupId,
                LeadId = followup.LeadId,
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
                    "Lead follow-up updated successfully. LeadFollowupId: {LeadFollowupId}",
                    followup.LeadFollowupId);
            }
            else
            {
                _logger.LogWarning(
                    "Lead follow-up update failed or follow-up not found. LeadFollowupId: {LeadFollowupId}",
                    followup.LeadFollowupId);
            }

            return result;
        }

        // =====================================================
        // DELETE
        // =====================================================

        public async Task<bool> DeleteAsync(int followupId)
        {
            _logger.LogInformation(
                "Deleting lead follow-up. FollowupId: {FollowupId}",
                followupId);

            if (followupId <= 0)
            {
                _logger.LogWarning(
                    "Lead follow-up deletion failed: Invalid FollowupId.");

                throw new ArgumentException(
                    "Invalid follow-up ID.");
            }

            var result =
                await _followupRepository.DeleteAsync(followupId);

            if (result)
            {
                _logger.LogInformation(
                    "Lead follow-up deleted successfully. FollowupId: {FollowupId}",
                    followupId);
            }
            else
            {
                _logger.LogWarning(
                    "Lead follow-up delete failed or follow-up not found. FollowupId: {FollowupId}",
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
                "Restoring lead follow-up. FollowupId: {FollowupId}",
                followupId);

            if (followupId <= 0)
            {
                _logger.LogWarning(
                    "Lead follow-up restore failed: Invalid FollowupId.");

                throw new ArgumentException(
                    "Invalid follow-up ID.");
            }

            var result =
                await _followupRepository.RestoreAsync(followupId);

            if (result)
            {
                _logger.LogInformation(
                    "Lead follow-up restored successfully. FollowupId: {FollowupId}",
                    followupId);
            }
            else
            {
                _logger.LogWarning(
                    "Lead follow-up restore failed or follow-up not found. FollowupId: {FollowupId}",
                    followupId);
            }

            return result;
        }

        // =====================================================
        // GET BY ID
        // =====================================================

        public async Task<LeadFollowupDto?> GetByIdAsync(
            int followupId)
        {
            _logger.LogInformation(
                "Getting lead follow-up by ID. FollowupId: {FollowupId}",
                followupId);

            if (followupId <= 0)
            {
                _logger.LogWarning(
                    "Get lead follow-up failed: Invalid FollowupId.");

                throw new ArgumentException(
                    "Invalid follow-up ID.");
            }

            var entity =
                await _followupRepository.GetByIdAsync(followupId);

            if (entity == null)
            {
                _logger.LogWarning(
                    "Lead follow-up not found. FollowupId: {FollowupId}",
                    followupId);

                return null;
            }

            return new LeadFollowupDto
            {
                LeadFollowupId = entity.LeadFollowupId,
                LeadId = entity.LeadId ?? 0,
                CandidateName = entity.CandidateName,
                FollowUpDate = entity.FollowUpDate,
                FollowUpBy = entity.FollowUpBy,
                Description = entity.Description,
                Status = entity.Status,
                NextFollowupDate = entity.NextFollowupDate,
                //CreatedAt = entity.CreatedAt,
              //  UpdatedAt = entity.UpdatedAt
            };
        }

        // =====================================================
        // GET ALL
        // =====================================================

        public async Task<IEnumerable<LeadFollowupDto>> GetAllAsync()
        {
            _logger.LogInformation(
                "Getting all active lead follow-ups.");

            var entities =
                await _followupRepository.GetAllAsync();

            var followups = entities.Select(entity => new LeadFollowupDto
            {
                LeadFollowupId = entity.LeadFollowupId,
                LeadId = entity.LeadId ?? 0,
                CandidateName = entity.CandidateName,
                FollowUpDate = entity.FollowUpDate,
                FollowUpBy = entity.FollowUpBy,
                Description = entity.Description,
                Status = entity.Status,
                NextFollowupDate = entity.NextFollowupDate,
                //CreatedAt = entity.CreatedAt,
                //UpdatedAt = entity.UpdatedAt
            });

            _logger.LogInformation(
                "Retrieved {Count} lead follow-ups.",
                followups.Count());

            return followups;
        }
    }
}