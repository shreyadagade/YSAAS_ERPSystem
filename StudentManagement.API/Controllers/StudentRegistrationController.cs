using Microsoft.AspNetCore.Mvc;
using StudentManagement.Application.Interfaces.Services.Registration;
using StudentManagement.Domain.Entities.Registration;

namespace StudentManagement.API.Controllers.Registration
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentRegistrationController : ControllerBase
    {
        private readonly IStudentRegistrationService _service;

        public StudentRegistrationController(
            IStudentRegistrationService service)
        {
            _service = service;
        }

        // 1. GET: api/StudentRegistration/{registrationId}
        [HttpGet("{registrationId}")]
        public async Task<IActionResult> GetById(int registrationId)
        {
            var registration =
                await _service.GetByIdAsync(registrationId);

            if (registration == null)
            {
                return NotFound(new
                {
                    message = "Student registration not found."
                });
            }

            return Ok(registration);
        }

        // 2. GET: api/StudentRegistration
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var registrations =
                await _service.GetAllAsync();

            return Ok(registrations);
        }

        // 3. POST: api/StudentRegistration
        [HttpPost]
        public async Task<IActionResult> Add(
            StudentRegistration registration)
        {
            var result =
                await _service.AddAsync(registration);

            return CreatedAtAction(
                nameof(GetById),
                new { registrationId = result.RegistrationId },
                result);
        }

        // 4. PUT: api/StudentRegistration/{registrationId}
        [HttpPut("{registrationId}")]
        public async Task<IActionResult> Update(
            int registrationId,
            StudentRegistration registration)
        {
            if (registrationId != registration.RegistrationId)
            {
                return BadRequest(new
                {
                    message = "Registration ID does not match."
                });
            }

            var existing =
                await _service.GetByIdAsync(registrationId);

            if (existing == null)
            {
                return NotFound(new
                {
                    message = "Student registration not found."
                });
            }

            await _service.UpdateAsync(registration);

            return Ok(new
            {
                message = "Student registration updated successfully."
            });
        }

        // 5. DELETE: api/StudentRegistration/{registrationId}
        [HttpDelete("{registrationId}")]
        public async Task<IActionResult> Delete(int registrationId)
        {
            var existing =
                await _service.GetByIdAsync(registrationId);

            if (existing == null)
            {
                return NotFound(new
                {
                    message = "Student registration not found."
                });
            }

            await _service.DeleteAsync(registrationId);

            return Ok(new
            {
                message = "Student registration deleted successfully."
            });
        }
    }
}