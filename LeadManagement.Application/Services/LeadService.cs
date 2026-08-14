
using LeadManagement.Application.DTOs;
using LeadManagement.Application.Interfaces.Repositories;
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

            // 1. Validate candidate name
            if (string.IsNullOrWhiteSpace(lead.CandidateName))
            {
                _logger.LogWarning("Lead creation failed: Candidate name is required.");
                throw new ArgumentException("Candidate name is required.");
            }

            if (lead.CandidateName.Length > 100)
            {
                _logger.LogWarning("Lead creation failed: Candidate name exceeds 100 characters.");
                throw new ArgumentException("Candidate name cannot exceed 100 characters.");
            }

            // 2. Validate email
            if (string.IsNullOrWhiteSpace(lead.EmailAddress))
            {
                _logger.LogWarning("Lead creation failed: Email address is required.");
                throw new ArgumentException("Email address is required.");
            }

            if (!IsValidEmail(lead.EmailAddress))
            {
                _logger.LogWarning(
                    "Lead creation failed: Invalid email address {Email}",
                    lead.EmailAddress);

                throw new ArgumentException("Please enter a valid email address.");
            }

            // 3. Validate mobile
            if (string.IsNullOrWhiteSpace(lead.MobileNumber))
            {
                _logger.LogWarning("Lead creation failed: Mobile number is required.");
                throw new ArgumentException("Mobile number is required.");
            }

            if (!IsValidMobile(lead.MobileNumber))
            {
                _logger.LogWarning("Lead creation failed: Invalid mobile number.");
                throw new ArgumentException(
                    "Mobile number must contain exactly 10 digits.");
            }

            // 4. Validate training type
            if (string.IsNullOrWhiteSpace(lead.TrainingType))
            {
                _logger.LogWarning("Lead creation failed: Training type is required.");
                throw new ArgumentException("Training type is required.");
            }

            // 5. Check duplicate email
            if (await _leadRepository.EmailExistsAsync(lead.EmailAddress))
            {
                _logger.LogWarning(
                    "Lead creation failed: Duplicate email {Email}",
                    lead.EmailAddress);

                throw new ArgumentException(
                    "A lead with this email address already exists.");
            }

            // 6. Check duplicate mobile
            if (await _leadRepository.MobileExistsAsync(lead.MobileNumber))
            {
                _logger.LogWarning(
                    "Lead creation failed: Duplicate mobile number.");

                throw new ArgumentException(
                    "A lead with this mobile number already exists.");
            }

            // 7. Convert DTO → Entity
            var entity = new TblLead
            {
                CandidateName = lead.CandidateName.Trim(),
                EmailAddress = lead.EmailAddress.Trim(),
                MobileNumber = lead.MobileNumber.Trim(),
                TrainingType = lead.TrainingType.Trim(),
                Description = lead.Description,
                LeadDate = lead.LeadDate
            };

            // 8. Save
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

            // 1. Validate Lead ID
            if (lead.LeadId <= 0)
            {
                _logger.LogWarning("Lead update failed: Invalid LeadId.");
                throw new ArgumentException("Invalid lead ID.");
            }

            // 2. Validate candidate name
            if (string.IsNullOrWhiteSpace(lead.CandidateName))
                throw new ArgumentException("Candidate name is required.");

            if (lead.CandidateName.Length > 100)
                throw new ArgumentException(
                    "Candidate name cannot exceed 100 characters.");

            // 3. Validate email
            if (string.IsNullOrWhiteSpace(lead.EmailAddress))
                throw new ArgumentException("Email address is required.");

            if (!IsValidEmail(lead.EmailAddress))
                throw new ArgumentException("Please enter a valid email address.");

            // 4. Validate mobile
            if (string.IsNullOrWhiteSpace(lead.MobileNumber))
                throw new ArgumentException("Mobile number is required.");

            if (!IsValidMobile(lead.MobileNumber))
                throw new ArgumentException(
                    "Mobile number must contain exactly 10 digits.");

            // 5. Validate training type
            if (string.IsNullOrWhiteSpace(lead.TrainingType))
                throw new ArgumentException("Training type is required.");

            // 6. Check duplicate email
            if (await _leadRepository.EmailExistsAsync(
                lead.EmailAddress,
                lead.LeadId))
            {
                _logger.LogWarning(
                    "Lead update failed: Duplicate email. LeadId: {LeadId}",
                    lead.LeadId);

                throw new ArgumentException(
                    "Another lead with this email address already exists.");
            }

            // 7. Check duplicate mobile
            if (await _leadRepository.MobileExistsAsync(
                lead.MobileNumber,
                lead.LeadId))
            {
                _logger.LogWarning(
                    "Lead update failed: Duplicate mobile. LeadId: {LeadId}",
                    lead.LeadId);

                throw new ArgumentException(
                    "Another lead with this mobile number already exists.");
            }

            // 8. Convert DTO → Entity
            var entity = new TblLead
            {
                LeadId = lead.LeadId,
                CandidateName = lead.CandidateName.Trim(),
                EmailAddress = lead.EmailAddress.Trim(),
                MobileNumber = lead.MobileNumber.Trim(),
                TrainingType = lead.TrainingType.Trim(),
                Description = lead.Description,
                LeadDate = lead.LeadDate
            };

            // 9. Update
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
                throw new ArgumentException("Invalid lead ID.");

            var result = await _leadRepository.DeleteAsync(leadId);

            if (result)
                _logger.LogInformation(
                    "Lead deleted successfully. LeadId: {LeadId}",
                    leadId);
            else
                _logger.LogWarning(
                    "Lead delete failed or lead not found. LeadId: {LeadId}",
                    leadId);

            return result;
        }

        public async Task<bool> RestoreAsync(int leadId)
        {
            _logger.LogInformation(
                "Restoring lead. LeadId: {LeadId}",
                leadId);

            if (leadId <= 0)
                throw new ArgumentException("Invalid lead ID.");

            var result = await _leadRepository.RestoreAsync(leadId);

            if (result)
                _logger.LogInformation(
                    "Lead restored successfully. LeadId: {LeadId}",
                    leadId);
            else
                _logger.LogWarning(
                    "Lead restore failed or lead not found. LeadId: {LeadId}",
                    leadId);

            return result;
        }

        public async Task<LeadDto?> GetByIdAsync(int leadId)
        {
            _logger.LogInformation(
                "Getting lead by ID. LeadId: {LeadId}",
                leadId);

            if (leadId <= 0)
                throw new ArgumentException("Invalid lead ID.");

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
                LeadDate = entity.LeadDate,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                DeletedAt = entity.DeletedAt,
                RestoredAt = entity.RestoredAt
            };
        }

        public async Task<IEnumerable<LeadDto>> GetAllAsync()
        {
            _logger.LogInformation("Getting all active leads.");

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
                LeadDate = entity.LeadDate,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                DeletedAt = entity.DeletedAt,
                RestoredAt = entity.RestoredAt
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

