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
    }
}