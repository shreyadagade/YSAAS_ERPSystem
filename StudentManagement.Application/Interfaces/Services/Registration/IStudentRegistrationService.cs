using StudentManagement.Application.DTOs.Registration;

namespace StudentManagement.Application.Interfaces.Services.Registration
{
    public interface IStudentRegistrationService
    {
        // Get registration by ID
        Task<StudentRegistrationResponseDto?> GetByIdAsync(
            int registrationId);

        // Get all registrations
        Task<IEnumerable<StudentRegistrationResponseDto>> GetAllAsync();

        // Create registration
        Task<StudentRegistrationResponseDto> AddAsync(
            StudentRegistrationRequestDto request);

        // Update registration
        Task UpdateAsync(
            int registrationId,
            StudentRegistrationRequestDto request);

        // Soft delete registration
        Task DeleteAsync(
            int registrationId);

        // Restore registration
        Task RestoreAsync(
            int registrationId);
    }
}

