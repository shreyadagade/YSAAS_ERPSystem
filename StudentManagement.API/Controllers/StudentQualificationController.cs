using Microsoft.AspNetCore.Mvc;
using StudentManagement.Application.Interfaces.Services.Registration;
using StudentManagement.Domain.Entities.Registration;

namespace StudentManagement.API.Controllers.Registration
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

        // 1. GET: api/StudentQualification/{qualificationId}
        [HttpGet("{qualificationId}")]
        public async Task<IActionResult> GetById(int qualificationId)
        {
            var qualification =
                await _service.GetByIdAsync(qualificationId);

            if (qualification == null)
            {
                return NotFound(new
                {
                    message = "Student qualification not found."
                });
            }

            return Ok(qualification);
        }

        // 2. GET: api/StudentQualification
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var qualifications = await _service.GetAllAsync();

            return Ok(qualifications);
        }

        // 3. POST: api/StudentQualification
        [HttpPost]
        public async Task<IActionResult> Add(
            StudentQualification qualification)
        {
            var result =
                await _service.AddAsync(qualification);

            return CreatedAtAction(
                nameof(GetById),
                new { qualificationId = result.QualificationId },
                result);
        }

        // 4. PUT: api/StudentQualification/{qualificationId}
        [HttpPut("{qualificationId}")]
        public async Task<IActionResult> Update(
            int qualificationId,
            StudentQualification qualification)
        {
            if (qualificationId != qualification.QualificationId)
            {
                return BadRequest(new
                {
                    message = "Qualification ID does not match."
                });
            }

            var existing =
                await _service.GetByIdAsync(qualificationId);

            if (existing == null)
            {
                return NotFound(new
                {
                    message = "Student qualification not found."
                });
            }

            await _service.UpdateAsync(qualification);

            return Ok(new
            {
                message = "Student qualification updated successfully."
            });
        }

        // 5. DELETE: api/StudentQualification/{qualificationId}
        [HttpDelete("{qualificationId}")]
        public async Task<IActionResult> Delete(int qualificationId)
        {
            var existing =
                await _service.GetByIdAsync(qualificationId);

            if (existing == null)
            {
                return NotFound(new
                {
                    message = "Student qualification not found."
                });
            }

            await _service.DeleteAsync(qualificationId);

            return Ok(new
            {
                message = "Student qualification deleted successfully."
            });
        }
    }
}