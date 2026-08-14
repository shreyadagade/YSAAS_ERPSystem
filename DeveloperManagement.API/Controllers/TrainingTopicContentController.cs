using DeveloperManagement.Application.DTOs.TopicContent;
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
    public class TrainingTopicContentController : ControllerBase
    {
        private readonly ITrainingTopicContentService _contentService;

        public TrainingTopicContentController(ITrainingTopicContentService contentService)
        {
            _contentService = contentService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateTrainingTopicContentDto dto)
        {
            var result = await _contentService.CreateAsync(dto);

            return StatusCode(
                StatusCodes.Status201Created,
                new
                {
                    statusCode = StatusCodes.Status201Created,
                    message = "Training topic content created successfully.",
                    data = result
                });
        }

        [HttpPut("update/{contentId:int}")]
        public async Task<IActionResult> Update(int contentId,[FromBody] UpdateTrainingTopicContentDto dto)
        {
            await _contentService.UpdateAsync(contentId, dto);

            var updatedContent = await _contentService.GetByIdAsync(contentId);

            return Ok(new
            {
                statusCode = StatusCodes.Status200OK,
                message = "Training topic content updated successfully.",
                data = updatedContent
            });
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _contentService.GetAllAsync();

            return Ok(new
            {
                statusCode = StatusCodes.Status200OK,
                message = "Training topic contents retrieved successfully.",
                data = result
            });
        }

        [HttpGet("get-by-id/{contentId:int}")]
        public async Task<IActionResult> GetById(int contentId)
        {
            var result = await _contentService.GetByIdAsync(contentId);

            return Ok(new
            {
                statusCode = StatusCodes.Status200OK,
                message = "Training topic content retrieved successfully.",
                data = result
            });
        }

        [HttpGet("get-by-topic/{topicId:int}")]
        public async Task<IActionResult> GetByTopic(int topicId)
        {
            var result = await _contentService.GetByTopicAsync(topicId);

            return Ok(new
            {
                statusCode = StatusCodes.Status200OK,
                message = "Training topic contents retrieved successfully.",
                data = result
            });
        }

        [HttpDelete("delete/{contentId:int}")]
        public async Task<IActionResult> Delete(int contentId)
        {
            await _contentService.DeleteAsync(contentId);

            return Ok(new
            {
                statusCode = StatusCodes.Status200OK,
                message = "Training topic content deleted successfully.",
                data = new
                {
                    contentId = contentId
                }
            });
        }

        [HttpPatch("restore/{contentId:int}")]
        public async Task<IActionResult> Restore(int contentId)
        {
            await _contentService.RestoreAsync(contentId);

            return Ok(new
            {
                statusCode = StatusCodes.Status200OK,
                message = "Training topic content restored successfully.",
                data = new
                {
                    contentId = contentId
                }
            });
        }

    }
}
