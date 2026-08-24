using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.DTOs.Branch;
using UserManagement.Application.Interfaces;

namespace UserManagement.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Super User")]
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private readonly IBranchService _branchService;

        public BranchController(IBranchService branchService)
        {
            _branchService = branchService;
        }


        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllBranches()
        {
            var result = await _branchService.GetAllAsync();

            return StatusCode(
                StatusCodes.Status200OK,
                new
                {
                    statusCode = StatusCodes.Status200OK,
                    message = "Branches retrieved successfully.",
                    data = result
                });
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetBranchById(int id)
        {
            var result = await _branchService.GetByIdAsync(id);

            return StatusCode(
                StatusCodes.Status200OK,
                new
                {
                    statusCode = StatusCodes.Status200OK,
                    message = "Branch retrieved successfully.",
                    data = result
                });
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateBranch([FromBody] CreateBranchDto dto)
        {
            var result = await _branchService.InsertAsync(dto);

            return StatusCode(
                StatusCodes.Status201Created,
                new
                {
                    statusCode = StatusCodes.Status201Created,
                    message = "Branch created successfully.",
                    data = result
                });
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateBranch(int id,[FromBody] UpdateBranchDto dto)
        {
            var result = await _branchService.UpdateAsync(id, dto);

            return StatusCode(
                StatusCodes.Status200OK,
                new
                {
                    statusCode = StatusCodes.Status200OK,
                    message = "Branch updated successfully.",
                    data = result
                });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            var result = await _branchService.DeleteAsync(id);

            return StatusCode(
                StatusCodes.Status200OK,
                new
                {
                    statusCode = StatusCodes.Status200OK,
                    message = "Branch deleted successfully.",
                    data = result
                });
        }


        [HttpPost("restore/{id}")]
        public async Task<IActionResult> RestoreBranch(int id)
        {
            var result = await _branchService.RestoreAsync(id);

            return StatusCode(
                StatusCodes.Status200OK,
                new
                {
                    statusCode = StatusCodes.Status200OK,
                    message = "Branch restored successfully.",
                    data = result
                });
        }
    }
}