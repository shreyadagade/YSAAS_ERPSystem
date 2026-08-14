using DeveloperManagement.Application.DTOs.Topic;
using DeveloperManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace DeveloperManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Roles = "Developer")]
    public class TrainingTopicController : ControllerBase
    {
        private readonly ITrainingTopicService _topicService;

        public TrainingTopicController(ITrainingTopicService topicService)
        {
            _topicService = topicService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(
            [FromBody] CreateTopicDto dto)
        {
            var result = await _topicService.CreateAsync(dto);

            return StatusCode(
                StatusCodes.Status201Created,
                new
                {
                    statusCode = 201,
                    message = "Topic created successfully.",
                    data = result
                });
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _topicService.GetAllAsync();

            return Ok(new
            {
                statusCode = 200,
                message = "Topics retrieved successfully.",
                data = result
            });
        }

        [HttpGet("get-by-id/{topicId:int}")]
        public async Task<IActionResult> GetById(int topicId)
        {
            var result = await _topicService.GetByIdAsync(topicId);

            return Ok(new
            {
                statusCode = 200,
                message = "Topic retrieved successfully.",
                data = result
            });
        }

        [HttpPut("update/{topicId:int}")]
        public async Task<IActionResult> Update(
            int topicId,
            [FromBody] UpdateTopicDto dto)
        {
            var result = await _topicService.UpdateAsync(
                topicId,
                dto);

            return Ok(new
            {
                statusCode = 200,
                message = "Topic updated successfully.",
                data = result
            });
        }

        [HttpDelete("delete/{topicId:int}")]
        public async Task<IActionResult> Delete(int topicId)
        {
            await _topicService.DeleteAsync(topicId);

            return Ok(new
            {
                statusCode = 200,
                message = "Topic deleted successfully."
            });
        }

        [HttpPatch("restore/{topicId:int}")]
        public async Task<IActionResult> Restore(int topicId)
        {
            await _topicService.RestoreAsync(topicId);

            return Ok(new
            {
                statusCode = 200,
                message = "Topic restored successfully."
            });
        }
    }
}