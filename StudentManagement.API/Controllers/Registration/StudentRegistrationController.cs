
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Application.DTOs.Registration;
using StudentManagement.Application.Interfaces.Services.Registration;

namespace StudentManagement.API.Controllers.Registration
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentRegistrationController
        : ControllerBase
    {
        private readonly IStudentRegistrationService _service;

        public StudentRegistrationController(
            IStudentRegistrationService service)
        {
            _service = service;
        }

        // GET: api/StudentRegistration
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var registrations =
                await _service.GetAllAsync();

            return Ok(registrations);
        }

        // GET: api/StudentRegistration/23
        [HttpGet("{registrationId}")]
        public async Task<IActionResult> GetById(
            int registrationId)
        {
            if (registrationId <= 0)
            {
                return BadRequest(new
                {
                    message =
                        "RegistrationId must be greater than 0."
                });
            }

            var registration =
                await _service.GetByIdAsync(
                    registrationId);

            if (registration == null)
            {
                return NotFound(new
                {
                    message =
                        "Registration not found."
                });
            }

            return Ok(registration);
        }

        // POST: api/StudentRegistration
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] StudentRegistrationRequestDto request)
        {
            var registration =
                await _service.AddAsync(
                    request);

            return Ok(registration);
        }

        // PUT: api/StudentRegistration/23
        [HttpPut("{registrationId}")]
        public async Task<IActionResult> Update(
            int registrationId,
            [FromBody] StudentRegistrationRequestDto request)
        {
            if (registrationId <= 0)
            {
                return BadRequest(new
                {
                    message =
                        "RegistrationId must be greater than 0."
                });
            }

            await _service.UpdateAsync(
                registrationId,
                request);

            return Ok(new
            {
                message =
                    "Student registration updated successfully."
            });
        }

        // DELETE: api/StudentRegistration/23
        [HttpDelete("{registrationId}")]
        public async Task<IActionResult> Delete(
            int registrationId)
        {
            if (registrationId <= 0)
            {
                return BadRequest(new
                {
                    message =
                        "RegistrationId must be greater than 0."
                });
            }

            await _service.DeleteAsync(
                registrationId);

            return Ok(new
            {
                message =
                    "Student registration deleted successfully."
            });
        }

        // POST: api/StudentRegistration/restore/23
        [HttpPost("restore/{registrationId}")]
        public async Task<IActionResult> Restore(
            int registrationId)
        {
            if (registrationId <= 0)
            {
                return BadRequest(new
                {
                    message =
                        "RegistrationId must be greater than 0."
                });
            }

            var restored =
                await _service.RestoreAsync(
                    registrationId);

            if (!restored)
            {
                return NotFound(new
                {
                    message =
                        "Registration not found or already restored."
                });
            }

            return Ok(new
            {
                message =
                    "Student registration restored successfully."
            });
        }
    }
}
