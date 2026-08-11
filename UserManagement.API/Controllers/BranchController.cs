using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.DTOs.Branch;
using UserManagement.Application.Interfaces;

namespace UserManagement.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
    Roles = "Trainer")]
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

            return Ok(result);
        }


        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetBranchById(int id)
        {
            if (id <= 0)
                return BadRequest(new
                {
                    message = "Invalid branch ID."
                });

            var result = await _branchService.GetByIdAsync(id);

            if (result == null)
                return NotFound(new
                {
                    message = "Branch not found."
                });

            return Ok(result);
        }


        [HttpPost("create")]
        public async Task<IActionResult> CreateBranch([FromBody] CreateBranchDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var result = await _branchService.InsertAsync(dto);

            return Ok(new
            {
                message = "Branch created successfully.",
                branchName = dto.BranchName
            });
        }


        [HttpPut("update")]
        public async Task<IActionResult> UpdateBranch([FromBody] UpdateBranchDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var existingBranch =
                await _branchService.GetByIdAsync(dto.BranchId);

            if (existingBranch == null)
                return NotFound(new
                {
                    message = "Branch not found."
                });

            var result = await _branchService.UpdateAsync(dto);

            return Ok(new
            {
                message = "Branch updated successfully."
            });
        }


        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            if (id <= 0)
                return BadRequest(new
                {
                    message = "Invalid branch ID."
                });

            var existingBranch =
                await _branchService.GetByIdAsync(id);

            if (existingBranch == null)
                return NotFound(new
                {
                    message = "Branch not found."
                });

            var result = await _branchService.DeleteAsync(id);

            return Ok(new
            {
                message = "Branch deleted successfully."
            });
        }


        [HttpPost("restore/{id}")]
        public async Task<IActionResult> RestoreBranch(int id)
        {
            if (id <= 0)
                return BadRequest(new
                {
                    message = "Invalid branch ID."
                });

            var result = await _branchService.RestoreAsync(id);

            return Ok(new
            {
                message = "Branch restored successfully."
            });
        }
    }
}