using LeadManagement.Application.DTOs.TrainingCourse;
using LeadManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LeadManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Counsellor, Super User")]

    public class TrainingCourseController : ControllerBase
    {
        private readonly ITrainingCourseService _courseService;

        public TrainingCourseController(ITrainingCourseService courseService)
        {
            _courseService = courseService;
        }

        // GET: api/TrainingCourse
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var courses = await _courseService.GetAllAsync();

            return Ok(courses);
        }

        // GET: api/TrainingCourse/1
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var course = await _courseService.GetByIdAsync(id);

            if (course == null)
                return NotFound();

            return Ok(course);
        }

        // POST: api/TrainingCourse
        [HttpPost("Create")]
        public async Task<IActionResult> Create(
            [FromBody] TrainingCourseDto course)
        {
            var courseId = await _courseService.CreateAsync(course);

            return Ok(new
            {
                message = "Course Created Successfully",
                courseId = courseId
            });
        }

        // PUT: api/TrainingCourse/1
        [HttpPut("Update/{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] TrainingCourseDto course)
        {
            course.CourseId = id;

            await _courseService.UpdateAsync(course);

            return Ok(new
            {
                message = "Course Updated Successfully"
            });
        }

        // DELETE: api/TrainingCourse/1
        [HttpDelete("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _courseService.DeleteAsync(id);

            return Ok(new
            {
                message = "Course Deleted Successfully"
            });
        }

        // PUT: api/TrainingCourse/restore/1
        [HttpPut("restore/{id:int}")]
        public async Task<IActionResult> Restore(int id)
        {
            await _courseService.RestoreAsync(id);

            return Ok(new
            {
                message = "Course Restored Successfully"
            });
        }
    }
}
