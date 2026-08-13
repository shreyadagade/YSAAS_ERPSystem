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
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto dto)
        {
            var result =
                await _roleService.CreateRoleAsync(dto);

            return Ok(new
            {
                message = result
            });
        }

        [HttpGet("get-roles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles =
                await _roleService.GetRolesAsync();

            return Ok(roles);
        }


        [HttpPut("update-role/{roleId}")]
        public async Task<IActionResult> UpdateRole(string roleId,[FromBody] UpdateRoleDto dto)
        {
            if (string.IsNullOrWhiteSpace(roleId))
            {
                return BadRequest(new
                {
                    message = "Role ID is required."
                });
            }

            var result = await _roleService.UpdateRoleAsync(roleId,dto);

            return Ok(new
            {
                message = result
            });
        }

        [HttpDelete("delete-role/{roleId}")]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            if (string.IsNullOrWhiteSpace(roleId))
            {
                return BadRequest(new
                {
                    message = "Role ID is required."
                });
            }

            var result = await _roleService.DeleteRoleAsync(roleId);

            return Ok(new
            {
                message = result
            });
        }
    }
}