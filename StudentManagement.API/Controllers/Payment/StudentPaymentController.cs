using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Application.DTOs.Payment;
using StudentManagement.Application.Interfaces.Services.Payment;

namespace StudentManagement.API.Controllers.Payment
    {
        [ApiController]
        [Route("api/[controller]")]
        [Authorize]
    public class StudentPaymentController : ControllerBase
        {
            private readonly IStudentPaymentService _service;

            public StudentPaymentController(
                IStudentPaymentService service)
            {
                _service = service;
            }

            // =====================================================
            // GET ALL PAYMENTS
            // =====================================================
            [HttpGet]
            public async Task<IActionResult> GetAll()
            {
                var payments =
                    await _service.GetAllAsync();

                return Ok(payments);
            }

            // =====================================================
            // GET PAYMENT BY ID
            // =====================================================
            [HttpGet("{paymentId:int}")]
            public async Task<IActionResult> GetById(
                int paymentId)
            {
                var payment =
                    await _service.GetByIdAsync(
                        paymentId);

                if (payment == null)
                {
                    return NotFound(
                        new
                        {
                            message = "Payment not found."
                        });
                }

                return Ok(payment);
            }

            // =====================================================
            // GET PAYMENT HISTORY BY REGISTRATION ID
            // =====================================================
            [HttpGet("registration/{registrationId:int}")]
            public async Task<IActionResult>
                GetByRegistrationId(
                    int registrationId)
            {
                var payments =
                    await _service.GetByRegistrationIdAsync(
                        registrationId);

                return Ok(payments);
            }

            // =====================================================
            // CREATE PAYMENT
            // =====================================================
            [HttpPost("Create")]
            public async Task<IActionResult> Create(
                StudentPaymentRequestDto request)
            {
                var result =
                    await _service.AddAsync(request);

                return CreatedAtAction(
                    nameof(GetById),
                    new
                    {
                        paymentId =
                            result.PaymentId
                    },
                    new
                    {
                        message =
                            "Payment created successfully.",
                        data = result
                    });
            }

            // =====================================================
            // UPDATE PAYMENT
            // =====================================================
            [HttpPut("Update/{paymentId:int}")]
            public async Task<IActionResult> Update(
                int paymentId,
                StudentPaymentRequestDto request)
            {
                await _service.UpdateAsync(
                    paymentId,
                    request);

                return Ok(
                    new
                    {
                        message =
                            "Payment updated successfully."
                    });
            }

            // =====================================================
            // DELETE PAYMENT
            // =====================================================
            [HttpDelete("Delete/{paymentId:int}")]
            public async Task<IActionResult> Delete(
                int paymentId)
            {
                await _service.DeleteAsync(
                    paymentId);

                return Ok(
                    new
                    {
                        message =
                            "Payment deleted successfully."
                    });
            }

            // =====================================================
            // RESTORE PAYMENT
            // =====================================================
            [HttpPut("{paymentId:int}/restore")]
            public async Task<IActionResult> Restore(
                int paymentId)
            {
                await _service.RestoreAsync(
                    paymentId);

                return Ok(
                    new
                    {
                        message =
                            "Payment restored successfully."
                    });
            }
        }
    }
