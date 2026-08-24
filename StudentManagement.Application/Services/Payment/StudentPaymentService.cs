using StudentManagement.Application.DTOs.Payment;
using StudentManagement.Application.Interfaces.Repositories.Payment;
using StudentManagement.Application.Interfaces.Services.Payment;
using StudentManagement.Domain.Entities.Payment;
using System;
using System.Collections.Generic;
using System.Text;


namespace StudentManagement.Application.Services.Payment
    {
        public class StudentPaymentService
            : IStudentPaymentService
        {
            private readonly IStudentPaymentRepository _repository;

            public StudentPaymentService(
                IStudentPaymentRepository repository)
            {
                _repository = repository;
            }

            // =====================================================
            // GET BY ID
            // =====================================================
            public async Task<StudentPaymentResponseDto?>
                GetByIdAsync(int paymentId)
            {
                if (paymentId <= 0)
                {
                    throw new ArgumentException(
                        "PaymentId must be greater than 0.");
                }

                var payment =
                    await _repository.GetByIdAsync(paymentId);

                if (payment == null)
                {
                    return null;
                }

                return MapToResponse(payment);
            }

            // =====================================================
            // GET ALL
            // =====================================================
            public async Task<IEnumerable<StudentPaymentResponseDto>>
                GetAllAsync()
            {
                var payments =
                    await _repository.GetAllAsync();

                return payments.Select(MapToResponse);
            }

            // =====================================================
            // GET PAYMENT HISTORY BY REGISTRATION ID
            // =====================================================
            public async Task<IEnumerable<StudentPaymentResponseDto>>
                GetByRegistrationIdAsync(
                    int registrationId)
            {
                if (registrationId <= 0)
                {
                    throw new ArgumentException(
                        "RegistrationId must be greater than 0.");
                }

                var payments =
                    await _repository.GetByRegistrationIdAsync(
                        registrationId);

                return payments.Select(MapToResponse);
            }

            // =====================================================
            // CREATE PAYMENT
            // =====================================================
            public async Task<StudentPaymentResponseDto>
                AddAsync(
                    StudentPaymentRequestDto request)
            {
                if (request == null)
                {
                    throw new ArgumentException(
                        "Payment data is required.");
                }

                ValidatePayment(request);

                var payment = new StudentPayment
                {
                    RegistrationId =
                        request.RegistrationId,

                    PaymentDate =
                        request.PaymentDate,

                    PaymentAmount =
                        request.PaymentAmount,

                    PaymentMode =
                        request.PaymentMode,

                    PaymentDescription =
                        request.PaymentDescription,

                   
                };

                var result =
                    await _repository.AddAsync(payment);

                return MapToResponse(result);
            }

            // =====================================================
            // UPDATE PAYMENT
            // =====================================================
            public async Task UpdateAsync(
                int paymentId,
                StudentPaymentRequestDto request)
            {
                if (paymentId <= 0)
                {
                    throw new ArgumentException(
                        "PaymentId must be greater than 0.");
                }

                if (request == null)
                {
                    throw new ArgumentException(
                        "Payment data is required.");
                }

                ValidatePayment(request);

                var existing =
                    await _repository.GetByIdAsync(paymentId);

                if (existing == null)
                {
                    throw new KeyNotFoundException(
                        "Payment not found.");
                }

                existing.RegistrationId =
                    request.RegistrationId;

                existing.PaymentDate =
                    request.PaymentDate;

                existing.PaymentAmount =
                    request.PaymentAmount;

                existing.PaymentMode =
                    request.PaymentMode;

                existing.PaymentDescription =
                    request.PaymentDescription;

               

                await _repository.UpdateAsync(existing);
            }

            // =====================================================
            // DELETE PAYMENT
            // =====================================================
            public async Task DeleteAsync(
                int paymentId)
            {
                if (paymentId <= 0)
                {
                    throw new ArgumentException(
                        "PaymentId must be greater than 0.");
                }

                var existing =
                    await _repository.GetByIdAsync(
                        paymentId);

                if (existing == null)
                {
                    throw new KeyNotFoundException(
                        "Payment not found.");
                }

                await _repository.DeleteAsync(
                    paymentId);
            }

            // =====================================================
            // RESTORE PAYMENT
            // =====================================================
            public async Task RestoreAsync(
                int paymentId)
            {
                if (paymentId <= 0)
                {
                    throw new ArgumentException(
                        "PaymentId must be greater than 0.");
                }

                await _repository.RestoreAsync(
                    paymentId);
            }

            // =====================================================
            // VALIDATION
            // =====================================================
            private static void ValidatePayment(
                StudentPaymentRequestDto request)
            {
                // Registration ID
                if (!request.RegistrationId.HasValue ||
                    request.RegistrationId.Value <= 0)
                {
                    throw new ArgumentException(
                        "RegistrationId must be greater than 0.");
                }

                // Payment Date
                if (!request.PaymentDate.HasValue)
                {
                    throw new ArgumentException(
                        "Payment date is required.");
                }

                // Payment Amount
                if (!request.PaymentAmount.HasValue ||
                    request.PaymentAmount.Value <= 0)
                {
                    throw new ArgumentException(
                        "Payment amount must be greater than 0.");
                }

                // Payment Mode
                if (string.IsNullOrWhiteSpace(
                    request.PaymentMode))
                {
                    throw new ArgumentException(
                        "Payment mode is required.");
                }

                if (request.PaymentMode.Length > 100)
                {
                    throw new ArgumentException(
                        "Payment mode cannot exceed 100 characters.");
                }

                // Payment Description
                if (!string.IsNullOrWhiteSpace(
                    request.PaymentDescription) &&
                    request.PaymentDescription.Length > 5000)
                {
                    throw new ArgumentException(
                        "Payment description cannot exceed 5000 characters.");
                }

                //// Is Paid
                //if (request.IsPaid.HasValue &&
                //    request.IsPaid.Value != 0 &&
                //    request.IsPaid.Value != 1)
                //{
                //    throw new ArgumentException(
                //        "IsPaid must be either 0 or 1.");
                //}
            }

            // =====================================================
            // ENTITY → RESPONSE DTO
            // =====================================================
            private static StudentPaymentResponseDto
                MapToResponse(
                    StudentPayment payment)
            {
                return new StudentPaymentResponseDto
                {
                    PaymentId =
                        payment.PaymentId,

                    RegistrationId =
                        payment.RegistrationId,

                    StudentName =
                        payment.StudentName,

                    CourseName =
                        payment.CourseName,

                    CourseFee =
                        payment.CourseFee,

                    TotalPaid =
                        payment.TotalPaid,

                    RemainingAmount =
                        payment.RemainingAmount,

                    PaymentAmount =
                        payment.PaymentAmount,

                    PaymentMode =
                        payment.PaymentMode,

                    PaymentDate =
                        payment.PaymentDate,

                    PaymentDescription =
                        payment.PaymentDescription,

                    IsPaid =
                        payment.IsPaid
                };
            }
        }
    }
