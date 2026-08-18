using StudentManagement.Application.DTOs.Payment;
using StudentManagement.Domain.Entities.Registration;

namespace StudentManagement.Application.Interfaces.Services.Registration
{
    public interface IStudentPaymentService
    {
        // CREATE PAYMENT
        Task<StudentPaymentResponseDto> CreateCoursePaymentAsync(
            StudentPaymentRequestDto request);

        // GET BY PAYMENT ID
        Task<StudentPayment?> GetByIdAsync(
            int paymentId);

        // GET ALL
        Task<IEnumerable<StudentPayment>> GetAllAsync();

        // GET ALL PAYMENT DETAILS
        Task<IEnumerable<StudentPaymentResponseDto>>
            GetAllPaymentDetailsAsync();

        // GET PAYMENT HISTORY BY REGISTRATION ID
        Task<IEnumerable<StudentPaymentResponseDto>>
            GetPaymentHistoryByRegistrationIdAsync(
                int registrationId);

        // ADD
        Task<StudentPayment> AddAsync(
            StudentPayment payment);

        // UPDATE
        Task UpdateAsync(
            StudentPayment payment);

        // DELETE
        Task DeleteAsync(
            int paymentId);
    }
}