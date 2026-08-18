using StudentManagement.Application.Interfaces.Repositories.Registration;
using StudentManagement.Application.Interfaces.Services.Registration;
using StudentManagement.Domain.Entities.Registration;

namespace StudentManagement.Application.Services.Registration
{
    public class StudentPaymentService : IStudentPaymentService
    {
        private readonly IStudentPaymentRepository _repository;

        public StudentPaymentService(
            IStudentPaymentRepository repository)
        {
            _repository = repository;
        }

        public async Task<StudentPayment?> GetByIdAsync(
            int paymentId)
        {
            return await _repository.GetByIdAsync(paymentId);
        }

        public async Task<IEnumerable<StudentPayment>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<StudentPayment> AddAsync(
            StudentPayment payment)
        {
            ValidatePayment(payment, false);

            return await _repository.AddAsync(payment);
        }

        public async Task UpdateAsync(
            StudentPayment payment)
        {
            if (payment.PaymentId <= 0)
            {
                throw new ArgumentException(
                    "PaymentId is required.");
            }

            ValidatePayment(payment, true);

            await _repository.UpdateAsync(payment);
        }

        public async Task DeleteAsync(int paymentId)
        {
            if (paymentId <= 0)
            {
                throw new ArgumentException(
                    "Invalid PaymentId.");
            }

            await _repository.DeleteAsync(paymentId);
        }

        public async Task RestoreAsync(int paymentId)
        {
            if (paymentId <= 0)
            {
                throw new ArgumentException(
                    "Invalid PaymentId.");
            }

            await _repository.RestoreAsync(paymentId);
        }

        private static void ValidatePayment(
            StudentPayment payment,
            bool isUpdate)
        {
            // RegistrationId
            if (!payment.RegistrationId.HasValue ||
                payment.RegistrationId <= 0)
            {
                throw new ArgumentException(
                    "RegistrationId is required.");
            }

            // Payment Date
            if (!payment.PaymentDate.HasValue)
            {
                throw new ArgumentException(
                    "PaymentDate is required.");
            }

            // Do not allow future payment date
            if (payment.PaymentDate.Value > DateTime.Now)
            {
                throw new ArgumentException(
                    "PaymentDate cannot be in the future.");
            }

            // Payment Amount
            if (!payment.PaymentAmount.HasValue)
            {
                throw new ArgumentException(
                    "PaymentAmount is required.");
            }

            if (payment.PaymentAmount <= 0)
            {
                throw new ArgumentException(
                    "PaymentAmount must be greater than zero.");
            }

            // Payment Mode
            if (string.IsNullOrWhiteSpace(payment.PaymentMode))
            {
                throw new ArgumentException(
                    "PaymentMode is required.");
            }

            if (payment.PaymentMode.Length > 100)
            {
                throw new ArgumentException(
                    "PaymentMode cannot exceed 100 characters.");
            }

            // Payment Description
            if (!string.IsNullOrWhiteSpace(
                payment.PaymentDescription))
            {
                if (payment.PaymentDescription.Length > 8000)
                {
                    throw new ArgumentException(
                        "PaymentDescription is too long.");
                }
            }

            // IsPaid
            if (!payment.IsPaid.HasValue)
            {
                throw new ArgumentException(
                    "IsPaid is required.");
            }

            if (payment.IsPaid != 0 &&
                payment.IsPaid != 1)
            {
                throw new ArgumentException(
                    "IsPaid must be either 0 or 1.");
            }
        }
    }
}