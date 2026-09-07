
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
                "Creating new lead. Email: {Email}, Mobile: {Mobile}",
                lead.EmailAddress,
                lead.MobileNumber);

            // Candidate Name validation
            if (string.IsNullOrWhiteSpace(lead.CandidateName))
            {
                _logger.LogWarning(
                    "Lead creation failed: Candidate name is required.");

                throw new ArgumentException(
                    "Candidate name is required.");
            }

            if (lead.CandidateName.Length > 100)
            {
                _logger.LogWarning(
                    "Lead creation failed: Candidate name exceeds 100 characters.");

                throw new ArgumentException(
                    "Candidate name cannot exceed 100 characters.");
            }

            // Email validation
            if (string.IsNullOrWhiteSpace(lead.EmailAddress))
            {
                _logger.LogWarning(
                    "Lead creation failed: Email address is required.");

                throw new ArgumentException(
                    "Email address is required.");
            }

            if (!IsValidEmail(lead.EmailAddress))
            {
                _logger.LogWarning(
                    "Lead creation failed: Invalid email address {Email}",
                    lead.EmailAddress);

                throw new ArgumentException(
                    "Please enter a valid email address.");
            }

            // Mobile validation
            if (string.IsNullOrWhiteSpace(lead.MobileNumber))
            {
                _logger.LogWarning(
                    "Lead creation failed: Mobile number is required.");

                throw new ArgumentException(
                    "Mobile number is required.");
            }

            if (!IsValidMobile(lead.MobileNumber))
            {
                _logger.LogWarning(
                    "Lead creation failed: Invalid mobile number.");

                throw new ArgumentException(
                    "Mobile number must contain exactly 10 digits.");
            }

            // Training Type validation
            if (string.IsNullOrWhiteSpace(lead.TrainingType))
            {
                _logger.LogWarning(
                    "Lead creation failed: Training type is required.");

                throw new ArgumentException(
                    "Training type is required.");
            }

            if (lead.TrainingType.Length > 20)
            {
                _logger.LogWarning(
                    "Lead creation failed: Training type exceeds 20 characters.");

                throw new ArgumentException(
                    "Training type cannot exceed 20 characters.");
            }

            // Status validation
            if (!string.IsNullOrWhiteSpace(lead.Status) &&
                lead.Status.Length > 20)
            {
                _logger.LogWarning(
                    "Lead creation failed: Status exceeds 20 characters.");

                throw new ArgumentException(
                    "Status cannot exceed 20 characters.");
            }

            // Duplicate Email check
            if (await _leadRepository.EmailExistsAsync(
                lead.EmailAddress.Trim()))
            {
                _logger.LogWarning(
                    "Lead creation failed: Duplicate email {Email}",
                    lead.EmailAddress);

                throw new ArgumentException(
                    "A lead with this email address already exists.");
            }

            // Duplicate Mobile check
            if (await _leadRepository.MobileExistsAsync(
                lead.MobileNumber.Trim()))
            {
                _logger.LogWarning(
                    "Lead creation failed: Duplicate mobile number.");

                throw new ArgumentException(
                    "A lead with this mobile number already exists.");
            }

            // Map DTO → Entity
            var entity = new TblLead
            {
                CandidateName = lead.CandidateName.Trim(),
                EmailAddress = lead.EmailAddress.Trim(),
                MobileNumber = lead.MobileNumber.Trim(),
                TrainingType = lead.TrainingType.Trim(),
                Description = lead.Description?.Trim(),
                Status = lead.Status?.Trim(),
                LeadDate = lead.LeadDate
            };

            // Insert lead
            var leadId = await _leadRepository.InsertAsync(entity);

            _logger.LogInformation(
                "Lead created successfully. LeadId: {LeadId}",
                leadId);

            return leadId;
        }

        public async Task<bool> UpdateAsync(LeadDto lead)
        {
            _logger.LogInformation(
                "Updating lead. LeadId: {LeadId}",
                lead.LeadId);

            // Lead ID validation
            if (lead.LeadId <= 0)
            {
                throw new ArgumentException(
                    "Invalid lead ID.");
            }

            // Candidate Name validation
            if (string.IsNullOrWhiteSpace(lead.CandidateName))
            {
                throw new ArgumentException(
                    "Candidate name is required.");
            }

            if (lead.CandidateName.Length > 100)
            {
                throw new ArgumentException(
                    "Candidate name cannot exceed 100 characters.");
            }

            // Email validation
            if (string.IsNullOrWhiteSpace(lead.EmailAddress))
            {
                throw new ArgumentException(
                    "Email address is required.");
            }

            if (!IsValidEmail(lead.EmailAddress))
            {
                throw new ArgumentException(
                    "Please enter a valid email address.");
            }

            // Mobile validation
            if (string.IsNullOrWhiteSpace(lead.MobileNumber))
            {
                throw new ArgumentException(
                    "Mobile number is required.");
            }

            if (!IsValidMobile(lead.MobileNumber))
            {
                throw new ArgumentException(
                    "Mobile number must contain exactly 10 digits.");
            }

            // Training Type validation
            if (string.IsNullOrWhiteSpace(lead.TrainingType))
            {
                throw new ArgumentException(
                    "Training type is required.");
            }

            if (lead.TrainingType.Length > 20)
            {
                throw new ArgumentException(
                    "Training type cannot exceed 20 characters.");
            }

            // Status validation
            if (!string.IsNullOrWhiteSpace(lead.Status) &&
                lead.Status.Length > 20)
            {
                throw new ArgumentException(
                    "Status cannot exceed 20 characters.");
            }

            // Duplicate Email check
            if (await _leadRepository.EmailExistsAsync(
                lead.EmailAddress.Trim(),
                lead.LeadId))
            {
                throw new ArgumentException(
                    "Another lead with this email address already exists.");
            }

            // Duplicate Mobile check
            if (await _leadRepository.MobileExistsAsync(
                lead.MobileNumber.Trim(),
                lead.LeadId))
            {
                throw new ArgumentException(
                    "Another lead with this mobile number already exists.");
            }

            // Map DTO → Entity
            var entity = new TblLead
            {
                LeadId = lead.LeadId,
                CandidateName = lead.CandidateName.Trim(),
                EmailAddress = lead.EmailAddress.Trim(),
                MobileNumber = lead.MobileNumber.Trim(),
                TrainingType = lead.TrainingType.Trim(),
                Description = lead.Description?.Trim(),
                Status = lead.Status?.Trim(),
                LeadDate = lead.LeadDate
            };

            // Update lead
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
            {
                throw new ArgumentException(
                    "Invalid lead ID.");
            }

            var result = await _leadRepository.DeleteAsync(leadId);

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
            {
                throw new ArgumentException(
                    "Invalid lead ID.");
            }

            var result = await _leadRepository.RestoreAsync(leadId);

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
            {
                throw new ArgumentException(
                    "Invalid lead ID.");
            }

            var entity = await _leadRepository.GetByIdAsync(leadId);

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
                LeadDate = entity.LeadDate
            };
        }

        public async Task<IEnumerable<LeadDto>> GetAllAsync()
        {
            _logger.LogInformation(
                "Getting all active leads.");

            var entities = await _leadRepository.GetAllAsync();

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
                LeadDate = entity.LeadDate
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

