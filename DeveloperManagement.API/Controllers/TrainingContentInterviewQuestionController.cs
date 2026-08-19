using DeveloperManagement.Application.DTOs.ContentInterviewQuestion;
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
    public class TrainingContentInterviewQuestionController : ControllerBase
    {
        private readonly ITrainingContentInterviewQuestionService
            _trainingContentInterviewQuestionService;

        public TrainingContentInterviewQuestionController(
            ITrainingContentInterviewQuestionService
                trainingContentInterviewQuestionService)
        {
            _trainingContentInterviewQuestionService =
                trainingContentInterviewQuestionService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateContentInterviewQuestionDto dto)
        {
            var result = await _trainingContentInterviewQuestionService.CreateAsync(dto);

            return StatusCode(
                StatusCodes.Status201Created,
                new
                {
                    statusCode = 201,
                    message = "Interview question created successfully.",
                    data = result
                });
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _trainingContentInterviewQuestionService
                    .GetAllAsync();

            return Ok(new
            {
                statusCode = 200,
                message = "Interview questions retrieved successfully.",
                data = result
            });
        }

        [HttpGet("get-by-id/{questionId:int}")]
        public async Task<IActionResult> GetById(int questionId)
        {
            var result = await _trainingContentInterviewQuestionService.GetByIdAsync(questionId);

            return Ok(new
            {
                statusCode = 200,
                message = "Interview question retrieved successfully.",
                data = result
            });
        }

        [HttpPut("update/{questionId:int}")]
        public async Task<IActionResult> Update(int questionId,
            [FromBody] UpdateContentInterviewQuestionDto dto)
        {
            var result = await _trainingContentInterviewQuestionService.UpdateAsync(questionId, dto);

            return Ok(new
            {
                statusCode = 200,
                message = "Interview question updated successfully.",
                data = result
            });
        }

        [HttpDelete("delete/{questionId:int}")]
        public async Task<IActionResult> Delete(int questionId)
        {
            await _trainingContentInterviewQuestionService.DeleteAsync(questionId);

            return Ok(new
            {
                statusCode = 200,
                message = "Interview question deleted successfully."
            });
        }

        [HttpPatch("restore/{questionId:int}")]
        public async Task<IActionResult> Restore(int questionId)
        {
            await _trainingContentInterviewQuestionService.RestoreAsync(questionId);

            return Ok(new
            {
                statusCode = 200,
                message = "Interview question restored successfully."
            });
        }
    }
}