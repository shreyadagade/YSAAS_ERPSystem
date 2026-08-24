using StudentManagement.Domain.Entities.Payment;
using System;
using System.Collections.Generic;
using System.Text;

    namespace StudentManagement.Application.Interfaces.Repositories.Payment
    {
        public interface IStudentPaymentRepository
        {
            Task<StudentPayment?> GetByIdAsync(
                int paymentId);

            Task<IEnumerable<StudentPayment>> GetAllAsync();

            Task<IEnumerable<StudentPayment>>
                GetByRegistrationIdAsync(
                    int registrationId);

            Task<StudentPayment> AddAsync(
                StudentPayment payment);

            Task UpdateAsync(
                StudentPayment payment);

            Task DeleteAsync(
                int paymentId);

            Task RestoreAsync(
                int paymentId);
        }
    }
