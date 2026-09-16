using LeadManagement.Application.DTOs.LeadFollowup;
using LeadManagement.Application.Interfaces.Repositories.LeadFollowup;
using LeadManagement.Application.Interfaces.Services;
using LeadManagement.Domain.Entities;
using LeadManagement.Domain.Entities.LeadFollowup;
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

            if (followup.LeadId <= 0)
            {
                throw new ArgumentException("Invalid lead ID.");
            }

            if (followup.FollowUpDate == null)
            {
                throw new ArgumentException(
                    "Follow-up date is required.");
            }

            if (string.IsNullOrWhiteSpace(followup.FollowUpBy))
            {
                throw new ArgumentException(
                    "Follow-up by is required.");
            }

            if (followup.FollowUpBy.Length > 100)
            {
                throw new ArgumentException(
                    "Follow-up by cannot exceed 100 characters.");
            }

            if (!string.IsNullOrWhiteSpace(followup.Description) &&
                followup.Description.Length > 8000)
            {
                throw new ArgumentException(
                    "Description cannot exceed 8000 characters.");
            }

            if (!string.IsNullOrWhiteSpace(followup.Status))
            {
                var status = followup.Status.Trim();

                if (status != "Hot" &&
                    status != "Warm" &&
                    status != "Cold")
                {
                    throw new ArgumentException(
                        "Status must be Hot, Warm, or Cold.");
                }
            }

            if (followup.NextFollowupDate != null &&
                followup.NextFollowupDate < followup.FollowUpDate)
            {
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
                "Lead follow-up created successfully. FollowupId: {FollowupId}",
                followupId);

            return followupId;
        }

        // =====================================================
        // UPDATE
        // =====================================================

        public async Task<bool> UpdateAsync(
            LeadFollowupDto followup)
        {
            _logger.LogInformation(
                "Updating lead follow-up. FollowupId: {FollowupId}",
                followup.LeadFollowupId);

            if (followup.LeadFollowupId <= 0)
            {
                throw new ArgumentException(
                    "Invalid follow-up ID.");
            }

            if (followup.LeadId <= 0)
            {
                throw new ArgumentException(
                    "Invalid lead ID.");
            }

            if (followup.FollowUpDate == null)
            {
                throw new ArgumentException(
                    "Follow-up date is required.");
            }

            if (string.IsNullOrWhiteSpace(followup.FollowUpBy))
            {
                throw new ArgumentException(
                    "Follow-up by is required.");
            }

            if (followup.FollowUpBy.Length > 100)
            {
                throw new ArgumentException(
                    "Follow-up by cannot exceed 100 characters.");
            }

            if (!string.IsNullOrWhiteSpace(followup.Description) &&
                followup.Description.Length > 8000)
            {
                throw new ArgumentException(
                    "Description cannot exceed 8000 characters.");
            }

            if (!string.IsNullOrWhiteSpace(followup.Status))
            {
                var status = followup.Status.Trim();

                if (status != "Hot" &&
                    status != "Warm" &&
                    status != "Cold")
                {
                    throw new ArgumentException(
                        "Status must be Hot, Warm, or Cold.");
                }
            }

            if (followup.NextFollowupDate != null &&
                followup.NextFollowupDate < followup.FollowUpDate)
            {
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
                    "Lead follow-up updated successfully. FollowupId: {FollowupId}",
                    followup.LeadFollowupId);
            }

            return result;
        }

        // =====================================================
        // DELETE
        // =====================================================

        public async Task<bool> DeleteAsync(int followupId)
        {
            if (followupId <= 0)
            {
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

            return result;
        }

        // =====================================================
        // RESTORE
        // =====================================================

        public async Task<bool> RestoreAsync(int followupId)
        {
            if (followupId <= 0)
            {
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

            return result;
        }

        // =====================================================
        // GET BY ID
        // =====================================================

        public async Task<LeadFollowupDto?> GetByIdAsync(
            int followupId)
        {
            if (followupId <= 0)
            {
                throw new ArgumentException(
                    "Invalid follow-up ID.");
            }

            var entity =
                await _followupRepository.GetByIdAsync(followupId);

            if (entity == null)
            {
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
                NextFollowupDate = entity.NextFollowupDate
            };
        }

        // =====================================================
        // GET ALL
        // =====================================================

        public async Task<IEnumerable<LeadFollowupDto>> GetAllAsync()
        {
            var entities =
                await _followupRepository.GetAllAsync();

            return entities.Select(entity => new LeadFollowupDto
            {
                LeadFollowupId = entity.LeadFollowupId,
                LeadId = entity.LeadId ?? 0,
                CandidateName = entity.CandidateName,
                FollowUpDate = entity.FollowUpDate,
                FollowUpBy = entity.FollowUpBy,
                Description = entity.Description,
                Status = entity.Status,
                NextFollowupDate = entity.NextFollowupDate
            });
        }

        // =====================================================
        // GET BY LEAD ID
        // =====================================================

        public async Task<IEnumerable<LeadFollowupDto>> GetByLeadIdAsync(
            int leadId)
        {
            _logger.LogInformation(
                "Getting lead follow-ups by LeadId: {LeadId}",
                leadId);

            if (leadId <= 0)
            {
                throw new ArgumentException(
                    "Invalid lead ID.");
            }

            var entities =
                await _followupRepository.GetByLeadIdAsync(leadId);

            return entities.Select(entity => new LeadFollowupDto
            {
                LeadFollowupId = entity.LeadFollowupId,
                LeadId = entity.LeadId ?? 0,
                CandidateName = entity.CandidateName,
                FollowUpDate = entity.FollowUpDate,
                FollowUpBy = entity.FollowUpBy,
                Description = entity.Description,
                Status = entity.Status,
                NextFollowupDate = entity.NextFollowupDate
            });
        }
    }
}