using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.DTOs.Role;
using UserManagement.Application.Interfaces;

namespace UserManagement.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Super User")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;

        public UserRoleController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto dto)
        {
            var result = await _userRoleService.AssignRoleAsync(dto);

            return Ok(new
            {
                message = result
            });
        }

        [HttpDelete("remove-assigned-role/{userId}/{roleId}")]
        public async Task<IActionResult> RemoveRole(string userId,string roleId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest(new
                {
                    message = "User ID is required."
                });
            }

            if (string.IsNullOrWhiteSpace(roleId))
            {
                return BadRequest(new
                {
                    message = "Role ID is required."
                });
            }

            var result = await _userRoleService.RemoveRoleAsync(userId,roleId);

            return Ok(new
            {
                message = result
            });
        }

        [AllowAnonymous]
        [HttpPost("get-user-roles")]
        public async Task<IActionResult> GetUserRoles([FromBody] GetUserRolesDto dto)
        {
            var roles =
                await _userRoleService.GetUserRolesAsync(dto);

            return Ok(roles);
        }
    }
}
