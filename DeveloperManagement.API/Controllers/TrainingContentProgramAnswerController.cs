using DeveloperManagement.Application.DTOs.ProgramAnswer;
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
    public class TrainingContentProgramAnswerController : ControllerBase
    {
        private readonly ITrainingContentProgramAnswerService
            _trainingContentProgramAnswerService;

        public TrainingContentProgramAnswerController(
            ITrainingContentProgramAnswerService trainingContentProgramAnswerService)
        {
            _trainingContentProgramAnswerService =
                trainingContentProgramAnswerService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(
            [FromBody] CreateProgramAnswerDto dto)
        {
            var result =
                await _trainingContentProgramAnswerService.CreateAsync(dto);

            return StatusCode(
                StatusCodes.Status201Created,
                new
                {
                    statusCode = 201,
                    message = "Program answer created successfully.",
                    data = result
                });
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result =
                await _trainingContentProgramAnswerService.GetAllAsync();

            return Ok(new
            {
                statusCode = 200,
                message = "Program answers retrieved successfully.",
                data = result
            });
        }

        [HttpGet("get-by-id/{programAnswerId:int}")]
        public async Task<IActionResult> GetById(int programAnswerId)
        {
            var result =
                await _trainingContentProgramAnswerService
                    .GetByIdAsync(programAnswerId);

            return Ok(new
            {
                statusCode = 200,
                message = "Program answer retrieved successfully.",
                data = result
            });
        }

        [HttpPut("update/{programAnswerId:int}")]
        public async Task<IActionResult> Update(
            int programAnswerId,
            [FromBody] UpdateProgramAnswerDto dto)
        {
            var result =
                await _trainingContentProgramAnswerService
                    .UpdateAsync(programAnswerId, dto);

            return Ok(new
            {
                statusCode = 200,
                message = "Program answer updated successfully.",
                data = result
            });
        }

        [HttpDelete("delete/{programAnswerId:int}")]
        public async Task<IActionResult> Delete(int programAnswerId)
        {
            await _trainingContentProgramAnswerService
                .DeleteAsync(programAnswerId);

            return Ok(new
            {
                statusCode = 200,
                message = "Program answer deleted successfully."
            });
        }

        [HttpPatch("restore/{programAnswerId:int}")]
        public async Task<IActionResult> Restore(int programAnswerId)
        {
            await _trainingContentProgramAnswerService
                .RestoreAsync(programAnswerId);

            return Ok(new
            {
                statusCode = 200,
                message = "Program answer restored successfully."
            });
        }
    }
}