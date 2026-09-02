
using StudentManagement.Application.DTOs.Registration;

namespace StudentManagement.Application.Interfaces.Services.Registration
{
    public interface IStudentRegistrationService
    {
        Task<StudentRegistrationResponseDto?> GetByIdAsync(
            int registrationId);

        Task<IEnumerable<StudentRegistrationResponseDto>> GetAllAsync();

        Task<StudentRegistrationResponseDto> AddAsync(
            StudentRegistrationRequestDto request);

        Task UpdateAsync(
            int registrationId,
            StudentRegistrationRequestDto request);

        Task DeleteAsync(
            int registrationId);

        Task<bool> RestoreAsync(
            int registrationId);
    }
}
