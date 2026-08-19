using DeveloperManagement.Application.DTOs.ContentQuestion;
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
    public class TrainingContentQuestionController : ControllerBase
    {
        private readonly ITrainingContentQuestionService
            _trainingContentQuestionService;

        public TrainingContentQuestionController(
            ITrainingContentQuestionService trainingContentQuestionService)
        {
            _trainingContentQuestionService =
                trainingContentQuestionService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateContentQuestionDto dto)
        {
            var result = await _trainingContentQuestionService.CreateAsync(dto);

            return StatusCode(
                StatusCodes.Status201Created,
                new
                {
                    statusCode = 201,
                    message = "Content question created successfully.",
                    data = result
                });
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _trainingContentQuestionService.GetAllAsync();

            return Ok(new
            {
                statusCode = 200,
                message = "Content questions retrieved successfully.",
                data = result
            });
        }

        [HttpGet("get-by-id/{questionId:int}")]
        public async Task<IActionResult> GetById(int questionId)
        {
            var result = await _trainingContentQuestionService
                    .GetByIdAsync(questionId);

            return Ok(new
            {
                statusCode = 200,
                message = "Content question retrieved successfully.",
                data = result
            });
        }

        [HttpPut("update/{questionId:int}")]
        public async Task<IActionResult> Update(int questionId,[FromBody] UpdateContentQuestionDto dto)
        {
            var result =
                await _trainingContentQuestionService
                    .UpdateAsync(questionId, dto);

            return Ok(new
            {
                statusCode = 200,
                message = "Content question updated successfully.",
                data = result
            });
        }

        [HttpDelete("delete/{questionId:int}")]
        public async Task<IActionResult> Delete(int questionId)
        {
            await _trainingContentQuestionService
                .DeleteAsync(questionId);

            return Ok(new
            {
                statusCode = 200,
                message = "Content question deleted successfully."
            });
        }

        [HttpPatch("restore/{questionId:int}")]
        public async Task<IActionResult> Restore(int questionId)
        {
            await _trainingContentQuestionService
                .RestoreAsync(questionId);

            return Ok(new
            {
                statusCode = 200,
                message = "Content question restored successfully."
            });
        }
    }
}