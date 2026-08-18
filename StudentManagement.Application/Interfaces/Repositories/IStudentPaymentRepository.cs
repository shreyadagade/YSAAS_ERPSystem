using StudentManagement.Application.DTOs.Payment;
using StudentManagement.Domain.Entities.Registration;

namespace StudentManagement.Application.Interfaces.Repositories.Registration
{
    public interface IStudentPaymentRepository
    {
        Task<StudentPayment?> GetByIdAsync(int paymentId);

        Task<IEnumerable<StudentPayment>> GetAllAsync();

        Task<StudentPayment> AddAsync(StudentPayment payment);

        Task UpdateAsync(StudentPayment payment);

        Task DeleteAsync(int paymentId);

        Task RestoreAsync(int paymentId);

        Task<decimal> GetTotalPaidAsync(int registrationId);

        Task<StudentPaymentResponseDto?> GetPaymentDetailsByIdAsync(
            int paymentId);

        Task<IEnumerable<StudentPaymentResponseDto>>
            GetAllPaymentDetailsAsync();

        Task<IEnumerable<StudentPaymentResponseDto>>
            GetPaymentHistoryByRegistrationIdAsync(
                int registrationId);
    }
}