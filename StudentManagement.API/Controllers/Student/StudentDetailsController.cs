using Microsoft.AspNetCore.Mvc;
using StudentManagement.Application.DTOs.Student;
using StudentManagement.Application.Interfaces.Services.Student;

namespace StudentManagement.API.Controllers.Student
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentDetailsController : ControllerBase
    {
        private readonly IStudentDetailsService _service;

        public StudentDetailsController(
            IStudentDetailsService service)
        {
            _service = service;
        }

        // =====================================================
        // 1. GET BY ID
        // GET: api/StudentDetails/{studentId}
        // =====================================================
        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetById(
            int studentId)
        {
            // CHANGE 1:
            // Validate StudentId
            if (studentId <= 0)
            {
                return BadRequest(new
                {
                    message = "StudentId must be greater than 0."
                });
            }

            var student =
                await _service.GetByIdAsync(studentId);

            // CHANGE 2:
            // StudentId is valid, but student does not exist
            if (student == null)
            {
                return NotFound(new
                {
                    message = "Student not found."
                });
            }

            return Ok(student);
        }

        // =====================================================
        // 2. GET ALL
        // GET: api/StudentDetails
        // =====================================================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var students =
                await _service.GetAllAsync();

            return Ok(students);
        }

        // =====================================================
        // 3. CREATE
        // POST: api/StudentDetails
        // =====================================================
        [HttpPost("Create")]
        public async Task<IActionResult> Create(
            StudentDetailsRequestDto request)
        {
            var result =
                await _service.AddAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new
                {
                    studentId = result.StudentId
                },
                new
                {
                    data = result,
                    message = "Student created successfully."
                });
        }

        // =====================================================
        // 4. UPDATE
        // PUT: api/StudentDetails/{studentId}
        // =====================================================
        [HttpPut("Update/{studentId}")]
        public async Task<IActionResult> Update(
            int studentId,
            StudentDetailsRequestDto request)
        {
            await _service.UpdateAsync(
                studentId,
                request);

            return Ok(new
            {
                message = "Student updated successfully."
            });
        }

        // =====================================================
        // 5. DELETE
        // DELETE: api/StudentDetails/{studentId}
        // =====================================================
        [HttpDelete("Delete/{studentId}")]
        public async Task<IActionResult> Delete(
            int studentId)
        {
            await _service.DeleteAsync(studentId);

            return Ok(new
            {
                message = "Student deleted successfully."
            });
        }

        // =====================================================
        // 6. RESTORE
        // POST: api/StudentDetails/restore/{studentId}
        // =====================================================
        [HttpPost("restore/{studentId}")]
       
        public async Task<IActionResult> Restore(
    int studentId)
        {
            if (studentId <= 0)
            {
                return BadRequest(new
                {
                    message = "StudentId must be greater than 0."
                });
            }

            var restored =
                await _service.RestoreAsync(studentId);

            if (!restored)
            {
                return NotFound(new
                {
                    message = "Student not found or already restored."
                });
            }

            return Ok(new
            {
                message = "Student restored successfully."
            });
        }
    }
}