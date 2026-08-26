using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.DTOs.Role;
using UserManagement.Application.Interfaces;

namespace UserManagement.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "Super User")]
    [ApiController]
    [Route("api/roles")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost("create-role")]
        public async Task<IActionResult> CreateRole(CreateRoleDto dto)
        {
            var result = await _roleService.CreateRoleAsync(dto);

            return StatusCode(
                StatusCodes.Status201Created,
                new
                {
                    statusCode = StatusCodes.Status201Created,
                    message = result
                });
        }

        [HttpGet("get-roles")]
        public async Task<IActionResult> GetRoles()
        {
            var result = await _roleService.GetRolesAsync();

            return StatusCode(
                StatusCodes.Status200OK,
                new
                {
                    statusCode = StatusCodes.Status200OK,
                    message = "Roles retrieved successfully.",
                    data = result
                });
        }

        [HttpPut("update-role/{roleId}")]
        public async Task<IActionResult> UpdateRole(
            string roleId,
            UpdateRoleDto dto)
        {
            var result =
                await _roleService.UpdateRoleAsync(roleId, dto);

            return StatusCode(
                StatusCodes.Status200OK,
                new
                {
                    statusCode = StatusCodes.Status200OK,
                    message = result
                });
        }

        [HttpDelete("delete-role/{roleId}")]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            var result =
                await _roleService.DeleteRoleAsync(roleId);

            return StatusCode(
                StatusCodes.Status200OK,
                new
                {
                    statusCode = StatusCodes.Status200OK,
                    message = result
                });
        }
    }
}