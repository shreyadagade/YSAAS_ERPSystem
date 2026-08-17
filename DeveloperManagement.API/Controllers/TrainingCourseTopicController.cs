using DeveloperManagement.Application.DTOs.CourseTopic;
using DeveloperManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Roles = "Developer")]
    public class TrainingCourseTopicController : ControllerBase
    {
        private readonly ITrainingCourseTopicService _courseTopicService;

        public TrainingCourseTopicController(ITrainingCourseTopicService courseTopicService)
        {
            _courseTopicService = courseTopicService;
        }

        [HttpPost("create-multiple")]
        public async Task<IActionResult> CreateMultiple([FromBody] CreateMultipleCourseTopicDto dto)
        {
            var result = await _courseTopicService.CreateMultipleAsync(dto);

            return StatusCode(
                StatusCodes.Status201Created,
                new
                {
                    statusCode = 201,
                    message = "Course topics created successfully.",
                    data = result
                });
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _courseTopicService.GetAllAsync();

            return Ok(new
            {
                statusCode = 200,
                message = "Course topics retrieved successfully.",
                data = result
            });
        }

        [HttpGet("get-by-id/{courseTopicId:int}")]
        public async Task<IActionResult> GetById(int courseTopicId)
        {
            var result =
                await _courseTopicService.GetByIdAsync(courseTopicId);

            return Ok(new
            {
                statusCode = 200,
                message = "Course topic retrieved successfully.",
                data = result
            });
        }

        [HttpGet("get-by-course/{courseId:int}")]
        public async Task<IActionResult> GetByCourse(int courseId)
        {
            var result =
                await _courseTopicService.GetByCourseAsync(courseId);

            return Ok(new
            {
                statusCode = 200,
                message = "Course topics retrieved successfully.",
                data = result
            });
        }

        [HttpPut("update/{courseId:int}")]
        public async Task<IActionResult> UpdateCourseTopics(int courseId,[FromBody] UpdateCourseTopicsDto dto)
        {
            var result = await _courseTopicService.UpdateCourseTopicsAsync(courseId,
                dto);

            return Ok(new
            {
                statusCode = 200,
                message = "Course topics updated successfully.",
                data = result
            });
        }

        [HttpDelete("delete/{courseTopicId:int}")]
        public async Task<IActionResult> Delete(int courseTopicId)
        {
            await _courseTopicService.DeleteAsync(courseTopicId);

            return Ok(new
            {
                statusCode = 200,
                message = "Course topic deleted successfully."
            });
        }

        [HttpPatch("restore/{courseTopicId:int}")]
        public async Task<IActionResult> Restore(int courseTopicId)
        {
            await _courseTopicService.RestoreAsync(courseTopicId);

            return Ok(new
            {
                statusCode = 200,
                message = "Course topic restored successfully."
            });
        }
    }
}