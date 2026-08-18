using Microsoft.AspNetCore.Mvc;
using StudentManagement.Application.Interfaces.Services.Registration;
using StudentManagement.Domain.Entities.Registration;

namespace StudentManagement.API.Controllers.Registration
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentPaymentController : ControllerBase
    {
        private readonly IStudentPaymentService _service;

        public StudentPaymentController(IStudentPaymentService service)
        {
            _service = service;
        }

        // 1. GET: api/StudentPayment/{paymentId}
        [HttpGet("{paymentId}")]
        public async Task<IActionResult> GetById(int paymentId)
        {
            var payment = await _service.GetByIdAsync(paymentId);

            if (payment == null)
            {
                return NotFound(new
                {
                    message = "Student payment not found."
                });
            }

            return Ok(payment);
        }

        // 2. GET: api/StudentPayment
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var payments = await _service.GetAllAsync();

            return Ok(payments);
        }

        // 3. POST: api/StudentPayment
        [HttpPost]
        public async Task<IActionResult> Add(StudentPayment payment)
        {
            var result = await _service.AddAsync(payment);

            return CreatedAtAction(
                nameof(GetById),
                new { paymentId = result.PaymentId },
                result);
        }

        // 4. PUT: api/StudentPayment/{paymentId}
        [HttpPut("{paymentId}")]
        public async Task<IActionResult> Update(
            int paymentId,
            StudentPayment payment)
        {
            if (paymentId != payment.PaymentId)
            {
                return BadRequest(new
                {
                    message = "Payment ID does not match."
                });
            }

            var existing = await _service.GetByIdAsync(paymentId);

            if (existing == null)
            {
                return NotFound(new
                {
                    message = "Student payment not found."
                });
            }

            await _service.UpdateAsync(payment);

            return Ok(new
            {
                message = "Student payment updated successfully."
            });
        }

        // 5. DELETE: api/StudentPayment/{paymentId}
        [HttpDelete("{paymentId}")]
        public async Task<IActionResult> Delete(int paymentId)
        {
            var existing = await _service.GetByIdAsync(paymentId);

            if (existing == null)
            {
                return NotFound(new
                {
                    message = "Student payment not found."
                });
            }

            await _service.DeleteAsync(paymentId);

            return Ok(new
            {
                message = "Student payment deleted successfully."
            });
        }
    }
}