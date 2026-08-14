using StudentManagement.Domain.Entities.Registration;

namespace StudentManagement.Application.Interfaces.Services.Registration
{
    public interface IStudentPaymentService
    {
        Task<StudentPayment?> GetByIdAsync(int paymentId);

        Task<IEnumerable<StudentPayment>> GetAllAsync();

        Task<StudentPayment> AddAsync(StudentPayment payment);

        Task UpdateAsync(StudentPayment payment);

        Task DeleteAsync(int paymentId);
    }
}