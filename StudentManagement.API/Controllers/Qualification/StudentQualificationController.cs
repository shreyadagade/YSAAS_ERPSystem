using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Application.DTOs.Qualification;
using StudentManagement.Application.Interfaces.Services.Qualification;


namespace StudentManagement.API.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class StudentQualificationController : ControllerBase
        {
            private readonly IStudentQualificationService _service;

            public StudentQualificationController(
                IStudentQualificationService service)
            {
                _service = service;
            }

            // =====================================================
            // GET ALL
            // =====================================================
            [HttpGet]
            public async Task<IActionResult> GetAll()
            {
                var result =
                    await _service.GetAllAsync();

                return Ok(new
                {
                    message = "Qualifications retrieved successfully.",
                    data = result
                });
            }

            // =====================================================
            // GET BY ID
            // =====================================================
            [HttpGet("{qualificationId}")]
            public async Task<IActionResult> GetById(
                int qualificationId)
            {
                var result =
                    await _service.GetByIdAsync(
                        qualificationId);

                if (result == null)
                {
                    return NotFound(new
                    {
                        message = "Qualification not found."
                    });
                }

                return Ok(new
                {
                    message = "Qualification retrieved successfully.",
                    data = result
                });
            }

            // =====================================================
            // CREATE
            // =====================================================
            [HttpPost]
            public async Task<IActionResult> Create(
                StudentQualificationRequestDto request)
            {
                var result =
                    await _service.AddAsync(request);

                return Ok(new
                {
                    message = "Student qualification created successfully.",
                    data = result
                });
            }

            // =====================================================
            // UPDATE
            // =====================================================
            [HttpPut("{qualificationId}")]
            public async Task<IActionResult> Update(
                int qualificationId,
                StudentQualificationRequestDto request)
            {
                await _service.UpdateAsync(
                    qualificationId,
                    request);

                return Ok(new
                {
                    message = "Student qualification updated successfully."
                });
            }

            // =====================================================
            // DELETE
            // =====================================================
            [HttpDelete("{qualificationId}")]
            public async Task<IActionResult> Delete(
                int qualificationId)
            {
                await _service.DeleteAsync(
                    qualificationId);

                return Ok(new
                {
                    message = "Student qualification deleted successfully."
                });
            }

            // =====================================================
            // RESTORE
            // =====================================================
            [HttpPut("{qualificationId}/restore")]
            public async Task<IActionResult> Restore(
                int qualificationId)
            {
                await _service.RestoreAsync(
                    qualificationId);

                return Ok(new
                {
                    message = "Student qualification restored successfully."
                });
            }
        }
    }
