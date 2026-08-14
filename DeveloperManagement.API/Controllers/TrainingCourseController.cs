using DeveloperManagement.Application.DTOs.Course;
using DeveloperManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Roles = "Developer")]
    public class TrainingCourseController : ControllerBase
    {
        private readonly ITrainingCourseService _trainingCourseService;

        public TrainingCourseController(ITrainingCourseService trainingCourseService)
        {
            _trainingCourseService = trainingCourseService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(
            [FromBody] CreateTrainingCourseDto dto)
        {
            var result = await _trainingCourseService.CreateAsync(dto);

            return StatusCode(
                StatusCodes.Status201Created,
                new
                {
                    statusCode = 201,
                    message = "Training course created successfully.",
                    data = result
                });
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _trainingCourseService.GetAllAsync();

            return Ok(new
            {
                statusCode = 200,
                message = "Training courses retrieved successfully.",
                data = result
            });
        }

        [HttpGet("get-by-id/{courseId:int}")]
        public async Task<IActionResult> GetById(int courseId)
        {
            var result = await _trainingCourseService.GetByIdAsync(courseId);

            return Ok(new
            {
                statusCode = 200,
                message = "Training course retrieved successfully.",
                data = result
            });
        }

        [HttpPut("update/{courseId:int}")]
        public async Task<IActionResult> Update(int courseId,[FromBody] UpdateTrainingCourseDto dto)
        {
            var result = await _trainingCourseService.UpdateAsync(courseId,dto);

            return Ok(new
            {
                statusCode = 200,
                message = "Training course updated successfully.",
                data = result
            });
        }

        [HttpDelete("delete/{courseId:int}")]
        public async Task<IActionResult> Delete(int courseId)
        {
            await _trainingCourseService.DeleteAsync(courseId);

            return Ok(new
            {
                statusCode = 200,
                message = "Training course deleted successfully."
            });
        }

        [HttpPatch("restore/{courseId:int}")]
        public async Task<IActionResult> Restore(int courseId)
        {
            await _trainingCourseService.RestoreAsync(courseId);

            return Ok(new
            {
                statusCode = 200,
                message = "Training course restored successfully."
            });
        }
    }
}