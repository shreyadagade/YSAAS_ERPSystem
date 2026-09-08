using LeadManagement.Application.DTOs.Lead;
using LeadManagement.Application.Interfaces.Repositories.Lead;
using LeadManagement.Application.Interfaces.Services;
using LeadManagement.Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Net.Mail;

namespace LeadManagement.Application.Services
{
    public class LeadService : ILeadService
    {
        private readonly ILeadRepository _leadRepository;
        private readonly ILogger<LeadService> _logger;

        public LeadService(
            ILeadRepository leadRepository,
            ILogger<LeadService> logger)
        {
            _leadRepository = leadRepository;
            _logger = logger;
        }

        public async Task<int> CreateAsync(LeadDto lead)
        {
            _logger.LogInformation(
                "Creating new lead. Email: {Email}, Mobile: {Mobile}, SourceId: {SourceId}",
                lead.EmailAddress,
                lead.MobileNumber,
                lead.SourceId);

            if (string.IsNullOrWhiteSpace(lead.CandidateName))
                throw new ArgumentException("Candidate name is required.");

            if (lead.CandidateName.Length > 100)
                throw new ArgumentException(
                    "Candidate name cannot exceed 100 characters.");

            if (string.IsNullOrWhiteSpace(lead.EmailAddress))
                throw new ArgumentException(
                    "Email address is required.");

            if (!IsValidEmail(lead.EmailAddress))
                throw new ArgumentException(
                    "Please enter a valid email address.");

            if (string.IsNullOrWhiteSpace(lead.MobileNumber))
                throw new ArgumentException(
                    "Mobile number is required.");

            if (!IsValidMobile(lead.MobileNumber))
                throw new ArgumentException(
                    "Mobile number must contain exactly 10 digits.");

            if (string.IsNullOrWhiteSpace(lead.TrainingType))
                throw new ArgumentException(
                    "Training type is required.");

            if (lead.TrainingType.Length > 20)
                throw new ArgumentException(
                    "Training type cannot exceed 20 characters.");

            if (!string.IsNullOrWhiteSpace(lead.Status) &&
                lead.Status.Length > 20)
            {
                throw new ArgumentException(
                    "Status cannot exceed 20 characters.");
            }

            if (lead.SourceId.HasValue &&
                lead.SourceId <= 0)
            {
                throw new ArgumentException(
                    "SourceId must be greater than 0.");
            }

            if (await _leadRepository.EmailExistsAsync(
                lead.EmailAddress.Trim()))
            {
                throw new ArgumentException(
                    "A lead with this email address already exists.");
            }

            if (await _leadRepository.MobileExistsAsync(
                lead.MobileNumber.Trim()))
            {
                throw new ArgumentException(
                    "A lead with this mobile number already exists.");
            }

            var entity = new TblLead
            {
                CandidateName = lead.CandidateName.Trim(),
                EmailAddress = lead.EmailAddress.Trim(),
                MobileNumber = lead.MobileNumber.Trim(),
                TrainingType = lead.TrainingType.Trim(),
                Description = lead.Description?.Trim(),
                Status = lead.Status?.Trim(),
                LeadDate = lead.LeadDate,
                SourceId = lead.SourceId
            };

            var leadId = await _leadRepository.InsertAsync(entity);

            _logger.LogInformation(
                "Lead created successfully. LeadId: {LeadId}, SourceId: {SourceId}",
                leadId,
                lead.SourceId);

            return leadId;
        }

        public async Task<bool> UpdateAsync(LeadDto lead)
        {
            _logger.LogInformation(
                "Updating lead. LeadId: {LeadId}, SourceId: {SourceId}",
                lead.LeadId,
                lead.SourceId);

            if (lead.LeadId <= 0)
                throw new ArgumentException("Invalid lead ID.");

            if (string.IsNullOrWhiteSpace(lead.CandidateName))
                throw new ArgumentException(
                    "Candidate name is required.");

            if (lead.CandidateName.Length > 100)
                throw new ArgumentException(
                    "Candidate name cannot exceed 100 characters.");

            if (string.IsNullOrWhiteSpace(lead.EmailAddress))
                throw new ArgumentException(
                    "Email address is required.");

            if (!IsValidEmail(lead.EmailAddress))
                throw new ArgumentException(
                    "Please enter a valid email address.");

            if (string.IsNullOrWhiteSpace(lead.MobileNumber))
                throw new ArgumentException(
                    "Mobile number is required.");

            if (!IsValidMobile(lead.MobileNumber))
                throw new ArgumentException(
                    "Mobile number must contain exactly 10 digits.");

            if (string.IsNullOrWhiteSpace(lead.TrainingType))
                throw new ArgumentException(
                    "Training type is required.");

            if (lead.TrainingType.Length > 20)
                throw new ArgumentException(
                    "Training type cannot exceed 20 characters.");

            if (!string.IsNullOrWhiteSpace(lead.Status) &&
                lead.Status.Length > 20)
            {
                throw new ArgumentException(
                    "Status cannot exceed 20 characters.");
            }

            if (lead.SourceId.HasValue &&
                lead.SourceId <= 0)
            {
                throw new ArgumentException(
                    "SourceId must be greater than 0.");
            }

            if (await _leadRepository.EmailExistsAsync(
                lead.EmailAddress.Trim(),
                lead.LeadId))
            {
                throw new ArgumentException(
                    "Another lead with this email address already exists.");
            }

            if (await _leadRepository.MobileExistsAsync(
                lead.MobileNumber.Trim(),
                lead.LeadId))
            {
                throw new ArgumentException(
                    "Another lead with this mobile number already exists.");
            }

            var entity = new TblLead
            {
                LeadId = lead.LeadId,
                CandidateName = lead.CandidateName.Trim(),
                EmailAddress = lead.EmailAddress.Trim(),
                MobileNumber = lead.MobileNumber.Trim(),
                TrainingType = lead.TrainingType.Trim(),
                Description = lead.Description?.Trim(),
                Status = lead.Status?.Trim(),
                LeadDate = lead.LeadDate,
                SourceId = lead.SourceId
            };

            var result = await _leadRepository.UpdateAsync(entity);

            if (result)
            {
                _logger.LogInformation(
                    "Lead updated successfully. LeadId: {LeadId}",
                    lead.LeadId);
            }
            else
            {
                _logger.LogWarning(
                    "Lead update failed or lead not found. LeadId: {LeadId}",
                    lead.LeadId);
            }

            return result;
        }

        public async Task<bool> DeleteAsync(int leadId)
        {
            _logger.LogInformation(
                "Deleting lead. LeadId: {LeadId}",
                leadId);

            if (leadId <= 0)
                throw new ArgumentException(
                    "Invalid lead ID.");

            var result =
                await _leadRepository.DeleteAsync(leadId);

            if (result)
            {
                _logger.LogInformation(
                    "Lead deleted successfully. LeadId: {LeadId}",
                    leadId);
            }
            else
            {
                _logger.LogWarning(
                    "Lead delete failed or lead not found. LeadId: {LeadId}",
                    leadId);
            }

            return result;
        }

        public async Task<bool> RestoreAsync(int leadId)
        {
            _logger.LogInformation(
                "Restoring lead. LeadId: {LeadId}",
                leadId);

            if (leadId <= 0)
                throw new ArgumentException(
                    "Invalid lead ID.");

            var result =
                await _leadRepository.RestoreAsync(leadId);

            if (result)
            {
                _logger.LogInformation(
                    "Lead restored successfully. LeadId: {LeadId}",
                    leadId);
            }
            else
            {
                _logger.LogWarning(
                    "Lead restore failed or lead not found. LeadId: {LeadId}",
                    leadId);
            }

            return result;
        }

        public async Task<LeadDto?> GetByIdAsync(int leadId)
        {
            _logger.LogInformation(
                "Getting lead by ID. LeadId: {LeadId}",
                leadId);

            if (leadId <= 0)
                throw new ArgumentException(
                    "Invalid lead ID.");

            var entity =
                await _leadRepository.GetByIdAsync(leadId);

            if (entity == null)
            {
                _logger.LogWarning(
                    "Lead not found. LeadId: {LeadId}",
                    leadId);

                return null;
            }

            return new LeadDto
            {
                LeadId = entity.LeadId,
                CandidateName = entity.CandidateName,
                EmailAddress = entity.EmailAddress,
                MobileNumber = entity.MobileNumber,
                TrainingType = entity.TrainingType,
                Description = entity.Description,
                Status = entity.Status,
                LeadDate = entity.LeadDate,
                SourceId = entity.SourceId
            };
        }

        public async Task<IEnumerable<LeadDto>> GetAllAsync()
        {
            _logger.LogInformation(
                "Getting all active leads.");

            var entities =
                await _leadRepository.GetAllAsync();

            _logger.LogInformation(
                "Retrieved {Count} leads.",
                entities.Count());

            return entities.Select(entity => new LeadDto
            {
                LeadId = entity.LeadId,
                CandidateName = entity.CandidateName,
                EmailAddress = entity.EmailAddress,
                MobileNumber = entity.MobileNumber,
                TrainingType = entity.TrainingType,
                Description = entity.Description,
                Status = entity.Status,
                LeadDate = entity.LeadDate,
                SourceId = entity.SourceId
            });
        }

        public async Task<IEnumerable<LeadDto>> GetBySourceIdAsync(
            int sourceId)
        {
            _logger.LogInformation(
                "Getting leads by SourceId. SourceId: {SourceId}",
                sourceId);

            if (sourceId <= 0)
                throw new ArgumentException(
                    "Invalid source ID.");

            var entities =
                await _leadRepository.GetBySourceIdAsync(sourceId);

            _logger.LogInformation(
                "Retrieved {Count} leads for SourceId: {SourceId}",
                entities.Count(),
                sourceId);

            return entities.Select(entity => new LeadDto
            {
                LeadId = entity.LeadId,
                CandidateName = entity.CandidateName,
                EmailAddress = entity.EmailAddress,
                MobileNumber = entity.MobileNumber,
                TrainingType = entity.TrainingType,
                Description = entity.Description,
                Status = entity.Status,
                LeadDate = entity.LeadDate,
                SourceId = entity.SourceId
            });
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new MailAddress(email);

                return mailAddress.Address.Equals(
                    email,
                    StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidMobile(string mobile)
        {
            return mobile.Length == 10 &&
                   mobile.All(char.IsDigit);
        }
    }
}