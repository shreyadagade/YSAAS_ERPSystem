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

        // =========================================================
        // 1. GET BY ID
        // GET: api/StudentDetails/{studentId}
        // =========================================================
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


        // =========================================================
        // 2. GET ALL
        // GET: api/StudentDetails
        // =========================================================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var students = await _service.GetAllAsync();

            return Ok(students);
        }


        // =========================================================
        // 3. CREATE
        // POST: api/StudentDetails
        // =========================================================
        [HttpPost]
        public async Task<IActionResult> Add(StudentDetails student)
        {
            var result = await _service.AddAsync(student);

            return CreatedAtAction(
                nameof(GetById),
                new
                {
                    studentId = result.StudentId
                },
                new
                {
                    message = "Student added successfully.",
                    data = result
                });
        }


        // =========================================================
        // 4. UPDATE
        // PUT: api/StudentDetails/{studentId}
        // =========================================================
        [HttpPut("{studentId}")]
        public async Task<IActionResult> Update(
            int studentId,
            StudentDetails student)
        {
            // Check URL ID and body ID
            if (studentId != student.StudentId)
            {
                return BadRequest(new
                {
                    message = "Student ID does not match."
                });
            }

            // Check whether student exists
            var existing = await _service.GetByIdAsync(studentId);

            if (existing == null)
            {
                return NotFound(new
                {
                    message = "Student not found."
                });
            }

            // Update student
            await _service.UpdateAsync(student);

            // Get updated record
            var updatedStudent = await _service.GetByIdAsync(studentId);

            return Ok(new
            {
                message = "Student updated successfully.",
                data = updatedStudent
            });
        }


        // =========================================================
        // 5. DELETE - SOFT DELETE
        // DELETE: api/StudentDetails/{studentId}
        // =========================================================
        [HttpDelete("{studentId}")]
        public async Task<IActionResult> Delete(int studentId)
        {
            // Check whether student exists
            var existing = await _service.GetByIdAsync(studentId);

            if (existing == null)
            {
                return NotFound(new
                {
                    message = "Student not found."
                });
            }

            // Soft delete
            await _service.DeleteAsync(studentId);

            return Ok(new
            {
                message = "Student deleted successfully.",
                studentId = studentId
            });
        }


        // =========================================================
        // 6. RESTORE
        // PUT: api/StudentDetails/restore/{studentId}
        // =========================================================
        [HttpPut("restore/{studentId}")]
        public async Task<IActionResult> Restore(int studentId)
        {
            // Restore the student
            await _service.RestoreAsync(studentId);

            // Get restored record
            var restoredStudent = await _service.GetByIdAsync(studentId);

            if (restoredStudent == null)
            {
                return NotFound(new
                {
                    message = "Student could not be restored or student not found."
                });
            }

            return Ok(new
            {
                message = "Student restored successfully.",
                data = restoredStudent
            });
        }
    }
}