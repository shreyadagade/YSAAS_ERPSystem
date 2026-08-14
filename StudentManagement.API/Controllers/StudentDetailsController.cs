using Microsoft.AspNetCore.Mvc;
using StudentManagement.Application.Interfaces.Services.Registration;
using StudentManagement.Domain.Entities.Registration;

namespace StudentManagement.API.Controllers.Registration
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentDetailsController : ControllerBase
    {
        private readonly IStudentDetailsService _service;

        public StudentDetailsController(IStudentDetailsService service)
        {
            _service = service;
        }

        // 1. GET: api/StudentDetails/{studentId}
        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetById(int studentId)
        {
            var student = await _service.GetByIdAsync(studentId);

            if (student == null)
            {
                return NotFound(new
                {
                    message = "Student not found."
                });
            }

            return Ok(student);
        }

        // 2. GET: api/StudentDetails
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var students = await _service.GetAllAsync();

            return Ok(students);
        }

        // 3. POST: api/StudentDetails
        [HttpPost]
        public async Task<IActionResult> Add(StudentDetails student)
        {
            var result = await _service.AddAsync(student);

            return CreatedAtAction(
                nameof(GetById),
                new { studentId = result.StudentId },
                result);
        }

        // 4. PUT: api/StudentDetails/{studentId}
        [HttpPut("{studentId}")]
        public async Task<IActionResult> Update(
            int studentId,
            StudentDetails student)
        {
            if (studentId != student.StudentId)
            {
                return BadRequest(new
                {
                    message = "Student ID does not match."
                });
            }

            var existing = await _service.GetByIdAsync(studentId);

            if (existing == null)
            {
                return NotFound(new
                {
                    message = "Student not found."
                });
            }

            await _service.UpdateAsync(student);

            return Ok(new
            {
                message = "Student updated successfully."
            });
        }

        // 5. DELETE: api/StudentDetails/{studentId}
        [HttpDelete("{studentId}")]
        public async Task<IActionResult> Delete(int studentId)
        {
            var existing = await _service.GetByIdAsync(studentId);

            if (existing == null)
            {
                return NotFound(new
                {
                    message = "Student not found."
                });
            }

            await _service.DeleteAsync(studentId);

            return Ok(new
            {
                message = "Student deleted successfully."
            });
        }

        // 6. PUT: api/StudentDetails/restore/{studentId}
        [HttpPut("restore/{studentId}")]
        public async Task<IActionResult> Restore(int studentId)
        {
            var existing = await _service.GetByIdAsync(studentId);

            if (existing == null)
            {
                return NotFound(new
                {
                    message = "Student not found."
                });
            }

            await _service.RestoreAsync(studentId);

            return Ok(new
            {
                message = "Student restored successfully."
            });
        }
    }
}