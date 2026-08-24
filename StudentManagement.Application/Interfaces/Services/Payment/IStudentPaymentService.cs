using StudentManagement.Application.DTOs.Payment;
using System;
using System.Collections.Generic;
using System.Text;



namespace StudentManagement.Application.Interfaces.Services.Payment
    {
        public interface IStudentPaymentService
        {
            Task<StudentPaymentResponseDto?> GetByIdAsync(
                int paymentId);

            Task<IEnumerable<StudentPaymentResponseDto>>
                GetAllAsync();

            Task<IEnumerable<StudentPaymentResponseDto>>
                GetByRegistrationIdAsync(
                    int registrationId);

            Task<StudentPaymentResponseDto> AddAsync(
                StudentPaymentRequestDto request);

            Task UpdateAsync(
                int paymentId,
                StudentPaymentRequestDto request);

            Task DeleteAsync(
                int paymentId);

            Task RestoreAsync(
                int paymentId);
        }
    }

