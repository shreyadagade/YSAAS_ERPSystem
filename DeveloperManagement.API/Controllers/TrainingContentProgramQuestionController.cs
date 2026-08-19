using DeveloperManagement.Application.DTOs.ProgramQuestion;
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
    public class TrainingContentProgramQuestionController : ControllerBase
    {
        private readonly ITrainingContentProgramQuestionService
            _trainingContentProgramQuestionService;

        public TrainingContentProgramQuestionController(
            ITrainingContentProgramQuestionService trainingContentProgramQuestionService)
        {
            _trainingContentProgramQuestionService =
                trainingContentProgramQuestionService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateProgramQuestionDto dto)
        {
            var result = await _trainingContentProgramQuestionService.CreateAsync(dto);

            return StatusCode(
                StatusCodes.Status201Created,
                new
                {
                    statusCode = 201,
                    message = "Program question created successfully.",
                    data = result
                });
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _trainingContentProgramQuestionService.GetAllAsync();

            return Ok(new
            {
                statusCode = 200,
                message = "Program questions retrieved successfully.",
                data = result
            });
        }

        [HttpGet("get-by-id/{programQuestionId:int}")]
        public async Task<IActionResult> GetById(int programQuestionId)
        {
            var result = await _trainingContentProgramQuestionService
                    .GetByIdAsync(programQuestionId);

            return Ok(new
            {
                statusCode = 200,
                message = "Program question retrieved successfully.",
                data = result
            });
        }

        [HttpPut("update/{programQuestionId:int}")]
        public async Task<IActionResult> Update(int programQuestionId,
            [FromBody] UpdateProgramQuestionDto dto)
        {
            var result = await _trainingContentProgramQuestionService
                    .UpdateAsync(programQuestionId, dto);

            return Ok(new
            {
                statusCode = 200,
                message = "Program question updated successfully.",
                data = result
            });
        }

        [HttpDelete("delete/{programQuestionId:int}")]
        public async Task<IActionResult> Delete(int programQuestionId)
        {
            await _trainingContentProgramQuestionService.DeleteAsync(programQuestionId);

            return Ok(new
            {
                statusCode = 200,
                message = "Program question deleted successfully."
            });
        }

        [HttpPatch("restore/{programQuestionId:int}")]
        public async Task<IActionResult> Restore(int programQuestionId)
        {
            await _trainingContentProgramQuestionService.RestoreAsync(programQuestionId);

            return Ok(new
            {
                statusCode = 200,
                message = "Program question restored successfully."
            });
        }
    }
}