using DeveloperManagement.Application.DTOs.Topic;
using DeveloperManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Developer")]
    public class TopicController : ControllerBase
    {
        private readonly ITopicService _topicService;
        public TopicController(ITopicService topicService)
        {
            _topicService = topicService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTopicDto dto)
        {
            var result = await _topicService.CreateAsync(dto);

            return StatusCode(
                StatusCodes.Status201Created,
                result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _topicService.GetAllAsync();

            return Ok(result);
        }

        [HttpGet("{topicId:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _topicService.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpPut("{topicId:int}")]
        public async Task<IActionResult> Update(int id,[FromBody] UpdateTopicDto dto)
        {
            var result = await _topicService.UpdateAsync(id, dto);

            return Ok(result);
        }

        [HttpDelete("{topicId:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _topicService.DeleteAsync(id);

            return Ok(new
            {
                statusCode = 200,
                message = "Topic deleted successfully."
            });
        }

        [HttpPatch("{topicId:int}/restore")]
        public async Task<IActionResult> Restore(int id)
        {
            await _topicService.RestoreAsync(id);

            return Ok(new
            {
                statusCode = 200,
                message = "Topic restored successfully."
            });
        }
    }
}
