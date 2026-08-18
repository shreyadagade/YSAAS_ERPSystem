using StudentManagement.Application.DTOs.Payment;
using StudentManagement.Application.Interfaces.Repositories.Registration;
using StudentManagement.Application.Interfaces.Services.Course;
using StudentManagement.Application.Interfaces.Services.Registration;
using StudentManagement.Domain.Entities.Registration;

namespace StudentManagement.Application.Services.Payment
{
    public class StudentPaymentService : IStudentPaymentService
    {
        private readonly IStudentPaymentRepository _paymentRepository;
        private readonly IStudentRegistrationService _registrationService;
        private readonly ICourseService _courseService;

        public StudentPaymentService(
            IStudentPaymentRepository paymentRepository,
            IStudentRegistrationService registrationService,
            ICourseService courseService)
        {
            _paymentRepository = paymentRepository;
            _registrationService = registrationService;
            _courseService = courseService;
        }


        // =========================================================
        // CREATE COURSE PAYMENT
        // =========================================================
        public async Task<StudentPaymentResponseDto>
            CreateCoursePaymentAsync(
                StudentPaymentRequestDto request)
        {
            // 1. Get registration
            var registration =
                await _registrationService.GetByIdAsync(
                    request.RegistrationId);

            if (registration == null)
            {
                throw new Exception(
                    "Registration not found.");
            }


            // 2. Check course
            if (!registration.CourseId.HasValue)
            {
                throw new Exception(
                    "Course is not assigned to this registration.");
            }


            // 3. Get course
            var course =
                await _courseService.GetCourseByIdAsync(
                    registration.CourseId.Value);

            if (course == null)
            {
                throw new Exception(
                    "Course not found.");
            }


            // 4. Check course fee
            if (!course.FeesAmount.HasValue)
            {
                throw new Exception(
                    "Course fee is not configured.");
            }


            decimal courseFee =
                Convert.ToDecimal(
                    course.FeesAmount.Value);


            // 5. Validate payment amount
            if (request.PaymentAmount <= 0)
            {
                throw new Exception(
                    "Payment amount must be greater than zero.");
            }


            // 6. Get previous payments
            decimal previousTotalPaid =
                await _paymentRepository
                    .GetTotalPaidAsync(
                        request.RegistrationId);


            // 7. Calculate new total
            decimal totalPaid =
                previousTotalPaid +
                request.PaymentAmount;


            // 8. Prevent excess payment
            if (totalPaid > courseFee)
            {
                decimal remainingBeforePayment =
                    courseFee -
                    previousTotalPaid;

                throw new Exception(
                    $"Payment amount exceeds the remaining course fee. " +
                    $"Remaining amount is {remainingBeforePayment}.");
            }


            // 9. Calculate remaining amount
            decimal remainingAmount =
                courseFee -
                totalPaid;


            // 10. Payment status
            int isPaid =
                remainingAmount == 0
                    ? 1
                    : 0;


            // 11. Create payment entity
            var payment =
                new StudentPayment
                {
                    RegistrationId =
                        request.RegistrationId,

                    PaymentDate =
                        DateTime.Now,

                    PaymentAmount =
                        Convert.ToDouble(
                            request.PaymentAmount),

                    PaymentMode =
                        request.PaymentMode,

                    PaymentDescription =
                        "Course payment",

                    IsPaid =
                        isPaid
                };


            // 12. Save payment
            var savedPayment =
                await _paymentRepository
                    .AddAsync(payment);


            // 13. Get saved payment details
            var paymentDetails =
                await _paymentRepository
                    .GetPaymentDetailsByIdAsync(
                        savedPayment.PaymentId);

            if (paymentDetails == null)
            {
                throw new Exception(
                    "Payment was created but details could not be retrieved.");
            }


            // 14. Return complete response
            return new StudentPaymentResponseDto
            {
                PaymentId =
                    paymentDetails.PaymentId,

                RegistrationId =
                    paymentDetails.RegistrationId,

                StudentName =
                    paymentDetails.StudentName,

                CourseName =
                    paymentDetails.CourseName,

                CourseFee =
                    courseFee,

                TotalPaid =
                    totalPaid,

                RemainingAmount =
                    remainingAmount,

                PaymentAmount =
                    request.PaymentAmount,

                PaymentMode =
                    request.PaymentMode,

                PaymentDate =
                    paymentDetails.PaymentDate,

                IsPaid =
                    isPaid
            };
        }


        // =========================================================
        // GET PAYMENT BY ID
        // =========================================================
        public async Task<StudentPayment?>
            GetByIdAsync(int paymentId)
        {
            return await _paymentRepository
                .GetByIdAsync(paymentId);
        }


        // =========================================================
        // GET ALL PAYMENTS
        // =========================================================
        public async Task<IEnumerable<StudentPayment>>
            GetAllAsync()
        {
            return await _paymentRepository
                .GetAllAsync();
        }


        // =========================================================
        // GET ALL PAYMENT DETAILS
        // =========================================================
        public async Task<IEnumerable<StudentPaymentResponseDto>>
            GetAllPaymentDetailsAsync()
        {
            return await _paymentRepository
                .GetAllPaymentDetailsAsync();
        }


        // =========================================================
        // GET PAYMENT HISTORY BY REGISTRATION ID
        // =========================================================
        public async Task<IEnumerable<StudentPaymentResponseDto>>
            GetPaymentHistoryByRegistrationIdAsync(
                int registrationId)
        {
            return await _paymentRepository
                .GetPaymentHistoryByRegistrationIdAsync(
                    registrationId);
        }


        // =========================================================
        // ADD
        // =========================================================
        public async Task<StudentPayment>
            AddAsync(StudentPayment payment)
        {
            return await _paymentRepository
                .AddAsync(payment);
        }


        // =========================================================
        // UPDATE
        // =========================================================
        public async Task UpdateAsync(
            StudentPayment payment)
        {
            await _paymentRepository
                .UpdateAsync(payment);
        }


        // =========================================================
        // DELETE
        // =========================================================
        public async Task DeleteAsync(
            int paymentId)
        {
            await _paymentRepository
                .DeleteAsync(paymentId);
        }
    }
}