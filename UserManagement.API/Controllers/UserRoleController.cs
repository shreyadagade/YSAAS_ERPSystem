using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.DTOs.Role;
using UserManagement.Application.Interfaces;

namespace UserManagement.API.Controllers
{
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
   // Roles = "Trainer")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;

        public UserRoleController(
            IUserRoleService userRoleService)
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

        [HttpDelete("remove-assigned-role")]
        public async Task<IActionResult> RemoveRole([FromBody] RemoveRoleDto dto)
        {
            var result =
                await _userRoleService.RemoveRoleAsync(dto);

            return Ok(new
            {
                message = result
            });
        }

        [HttpPost("get-user-roles")]
        public async Task<IActionResult> GetUserRoles([FromBody] GetUserRolesDto dto)
        {
            var roles =
                await _userRoleService.GetUserRolesAsync(dto);

            return Ok(roles);
        }
    }
}
