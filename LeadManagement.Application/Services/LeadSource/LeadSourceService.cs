using LeadManagement.Application.DTOs.LeadSource;
using LeadManagement.Application.Interfaces.Repositories.LeadSource;
using LeadManagement.Application.Interfaces.Services;
using LeadManagement.Domain.Entities.LeadSource;
using Microsoft.Extensions.Logging;

namespace LeadManagement.Application.Services
{
    public class LeadSourceService : ILeadSourceService
    {
        private readonly ILeadSourceRepository _sourceRepository;
        private readonly ILogger<LeadSourceService> _logger;

        public LeadSourceService(
            ILeadSourceRepository sourceRepository,
            ILogger<LeadSourceService> logger)
        {
            _sourceRepository = sourceRepository;
            _logger = logger;
        }

        // =====================================================
        // CREATE
        // =====================================================

        public async Task<int> CreateAsync(LeadSourceDto source)
        {
            _logger.LogInformation(
                "Creating lead source. SourceName: {SourceName}",
                source.SourceName);

            // 1. Source name validation
            if (string.IsNullOrWhiteSpace(source.SourceName))
            {
                _logger.LogWarning(
                    "Lead source creation failed: Source name is required.");

                throw new ArgumentException(
                    "Source name is required.");
            }

            // 2. Maximum length validation
            if (source.SourceName.Trim().Length > 100)
            {
                _logger.LogWarning(
                    "Lead source creation failed: Source name exceeds 100 characters.");

                throw new ArgumentException(
                    "Source name cannot exceed 100 characters.");
            }

            var sourceName = source.SourceName.Trim();

            // 3. Duplicate source name validation
            if (await _sourceRepository.SourceNameExistsAsync(sourceName))
            {
                _logger.LogWarning(
                    "Lead source creation failed: Duplicate source name {SourceName}",
                    sourceName);

                throw new Exception(
                    "A lead source with this name already exists.");
            }

            // 4. Create entity
            var entity = new TblLeadSource
            {
                SourceName = sourceName,
                Flag = 1
            };

            // 5. Save
            var sourceId =
                await _sourceRepository.InsertAsync(entity);

            _logger.LogInformation(
                "Lead source created successfully. SourceId: {SourceId}",
                sourceId);

            return sourceId;
        }

        // =====================================================
        // UPDATE
        // =====================================================

        public async Task<bool> UpdateAsync(LeadSourceDto source)
        {
            _logger.LogInformation(
                "Updating lead source. SourceId: {SourceId}",
                source.SourceId);

            // 1. Source ID validation
            if (source.SourceId <= 0)
            {
                _logger.LogWarning(
                    "Lead source update failed: Invalid SourceId.");

                throw new ArgumentException(
                    "Invalid source ID.");
            }

            // 2. Source name validation
            if (string.IsNullOrWhiteSpace(source.SourceName))
            {
                _logger.LogWarning(
                    "Lead source update failed: Source name is required.");

                throw new ArgumentException(
                    "Source name is required.");
            }

            // 3. Maximum length validation
            if (source.SourceName.Trim().Length > 100)
            {
                _logger.LogWarning(
                    "Lead source update failed: Source name exceeds 100 characters.");

                throw new ArgumentException(
                    "Source name cannot exceed 100 characters.");
            }

            var sourceName = source.SourceName.Trim();

            // 4. Duplicate source name validation
            if (await _sourceRepository.SourceNameExistsAsync(
                sourceName,
                source.SourceId))
            {
                _logger.LogWarning(
                    "Lead source update failed: Duplicate source name {SourceName}",
                    sourceName);

                throw new Exception(
                    "Another lead source with this name already exists.");
            }

            // 5. Create entity
            var entity = new TblLeadSource
            {
                SourceId = source.SourceId,
                SourceName = sourceName
            };

            // 6. Update
            var result =
                await _sourceRepository.UpdateAsync(entity);

            if (result)
            {
                _logger.LogInformation(
                    "Lead source updated successfully. SourceId: {SourceId}",
                    source.SourceId);
            }
            else
            {
                _logger.LogWarning(
                    "Lead source update failed or source not found. SourceId: {SourceId}",
                    source.SourceId);
            }

            return result;
        }

        // =====================================================
        // DELETE
        // =====================================================

        public async Task<bool> DeleteAsync(int sourceId)
        {
            _logger.LogInformation(
                "Deleting lead source. SourceId: {SourceId}",
                sourceId);

            if (sourceId <= 0)
            {
                _logger.LogWarning(
                    "Lead source deletion failed: Invalid SourceId.");

                throw new ArgumentException(
                    "Invalid source ID.");
            }

            var result =
                await _sourceRepository.DeleteAsync(sourceId);

            if (result)
            {
                _logger.LogInformation(
                    "Lead source deleted successfully. SourceId: {SourceId}",
                    sourceId);
            }
            else
            {
                _logger.LogWarning(
                    "Lead source delete failed or source not found. SourceId: {SourceId}",
                    sourceId);
            }

            return result;
        }

        // =====================================================
        // RESTORE
        // =====================================================

        public async Task<bool> RestoreAsync(int sourceId)
        {
            _logger.LogInformation(
                "Restoring lead source. SourceId: {SourceId}",
                sourceId);

            if (sourceId <= 0)
            {
                _logger.LogWarning(
                    "Lead source restore failed: Invalid SourceId.");

                throw new ArgumentException(
                    "Invalid source ID.");
            }

            var result =
                await _sourceRepository.RestoreAsync(sourceId);

            if (result)
            {
                _logger.LogInformation(
                    "Lead source restored successfully. SourceId: {SourceId}",
                    sourceId);
            }
            else
            {
                _logger.LogWarning(
                    "Lead source restore failed or source not found. SourceId: {SourceId}",
                    sourceId);
            }

            return result;
        }

        // =====================================================
        // GET BY ID
        // =====================================================

        public async Task<LeadSourceDto?> GetByIdAsync(int sourceId)
        {
            _logger.LogInformation(
                "Getting lead source by ID. SourceId: {SourceId}",
                sourceId);

            if (sourceId <= 0)
            {
                _logger.LogWarning(
                    "Get lead source failed: Invalid SourceId.");

                throw new ArgumentException(
                    "Invalid source ID.");
            }

            var entity =
                await _sourceRepository.GetByIdAsync(sourceId);

            if (entity == null)
            {
                _logger.LogWarning(
                    "Lead source not found. SourceId: {SourceId}",
                    sourceId);

                return null;
            }

            return new LeadSourceDto
            {
                SourceId = entity.SourceId,
                SourceName = entity.SourceName,
                Flag = entity.Flag,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                DeletedAt = entity.DeletedAt,
                RestoredAt = entity.RestoredAt
            };
        }

        // =====================================================
        // GET ALL
        // =====================================================

        public async Task<IEnumerable<LeadSourceDto>> GetAllAsync()
        {
            _logger.LogInformation(
                "Getting all active lead sources.");

            var entities =
                await _sourceRepository.GetAllAsync();

            var sources = entities.Select(entity =>
                new LeadSourceDto
                {
                    SourceId = entity.SourceId,
                    SourceName = entity.SourceName,
                    Flag = entity.Flag,
                    CreatedAt = entity.CreatedAt,
                    UpdatedAt = entity.UpdatedAt,
                    DeletedAt = entity.DeletedAt,
                    RestoredAt = entity.RestoredAt
                });

            _logger.LogInformation(
                "Retrieved {Count} lead sources.",
                sources.Count());

            return sources;
        }
    }
}