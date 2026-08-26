using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Application.DTOs.Registration;
using StudentManagement.Application.Interfaces.Services.Registration;

namespace StudentManagement.API.Controllers.Registration
    {
        [ApiController]
        [Route("api/[controller]")]
        [Authorize]
    public class StudentRegistrationController : ControllerBase
        {
            private readonly IStudentRegistrationService _service;

            public StudentRegistrationController(
                IStudentRegistrationService service)
            {
                _service = service;
            }

            // =====================================================
            // 1. GET ALL
            // GET: api/StudentRegistration
            // =====================================================
            [HttpGet]
            public async Task<IActionResult> GetAll()
            {
                var registrations =
                    await _service.GetAllAsync();

                return Ok(registrations);
            }

            // =====================================================
            // 2. GET BY ID
            // GET: api/StudentRegistration/{registrationId}
            // =====================================================
            [HttpGet("{registrationId}")]
            public async Task<IActionResult> GetById(
                int registrationId)
            {
                var registration =
                    await _service.GetByIdAsync(registrationId);

                if (registration == null)
                {
                    return NotFound(new
                    {
                        message = "Registration not found."
                    });
                }

                return Ok(registration);
            }

            // =====================================================
            // 3. CREATE
            // POST: api/StudentRegistration
            // =====================================================
            [HttpPost]
            public async Task<IActionResult> Create(
                StudentRegistrationRequestDto request)
            {
                var result =
                    await _service.AddAsync(request);

                return CreatedAtAction(
                    nameof(GetById),
                    new
                    {
                        registrationId =
                            result.RegistrationId
                    },
                    new
                    {
                        message =
                            "Student registration created successfully.",
                        data = result
                    });
            }

            // =====================================================
            // 4. UPDATE
            // PUT: api/StudentRegistration/{registrationId}
            // =====================================================
            [HttpPut("{registrationId}")]
            public async Task<IActionResult> Update(
                int registrationId,
                StudentRegistrationRequestDto request)
            {
                await _service.UpdateAsync(
                    registrationId,
                    request);

                return Ok(new
                {
                    message =
                        "Student registration updated successfully."
                });
            }

            // =====================================================
            // 5. DELETE
            // DELETE: api/StudentRegistration/{registrationId}
            // =====================================================
            [HttpDelete("{registrationId}")]
            public async Task<IActionResult> Delete(
                int registrationId)
            {
                await _service.DeleteAsync(
                    registrationId);

                return Ok(new
                {
                    message =
                        "Student registration deleted successfully."
                });
            }

            // =====================================================
            // 6. RESTORE
            // POST: api/StudentRegistration/restore/{registrationId}
            // =====================================================
            [HttpPost("restore/{registrationId}")]
            public async Task<IActionResult> Restore(
                int registrationId)
            {
                await _service.RestoreAsync(
                    registrationId);

                return Ok(new
                {
                    message =
                        "Student registration restored successfully."
                });
            }
        }
    }