using Microsoft.AspNetCore.Mvc;
using StudentManagement.Application.DTOs.Payment;
using StudentManagement.Application.Interfaces.Services.Registration;

namespace StudentManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentPaymentController : ControllerBase
    {
        private readonly IStudentPaymentService _paymentService;

        public StudentPaymentController(
            IStudentPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // ==========================================
        // CREATE COURSE PAYMENT
        // ==========================================
        [HttpPost("course-payment")]
        public async Task<IActionResult> CreateCoursePayment(
            [FromBody] StudentPaymentRequestDto request)
        {
            var result =
                await _paymentService.CreateCoursePaymentAsync(request);

            return Ok(result);
        }


        // ==========================================
        // GET ALL PAYMENT DETAILS
        // ==========================================
        [HttpGet("all-details")]
        public async Task<IActionResult> GetAllPaymentDetails()
        {
            var payments =
                await _paymentService.GetAllPaymentDetailsAsync();

            return Ok(payments);
        }


        // ==========================================
        // GET PAYMENT HISTORY BY REGISTRATION ID
        // ==========================================
        [HttpGet("history/{registrationId}")]
        public async Task<IActionResult> GetPaymentHistory(
            int registrationId)
        {
            var payments =
                await _paymentService
                    .GetPaymentHistoryByRegistrationIdAsync(
                        registrationId);

            if (payments == null || !payments.Any())
            {
                return NotFound(
                    new
                    {
                        statusCode = 404,
                        message =
                            "No payment history found for this registration."
                    });
            }

            return Ok(payments);
        }
    }
}