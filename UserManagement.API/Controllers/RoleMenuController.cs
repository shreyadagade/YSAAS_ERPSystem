using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.DTOs.RoleMenu;
using UserManagement.Application.Interfaces;

namespace UserManagement.API.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Super User")]
    [ApiController]
    [Route("api/role-menu")]
    public class RoleMenuController : ControllerBase
    {
        private readonly IRoleMenuService _roleMenuService;

        public RoleMenuController(IRoleMenuService roleMenuService)
        {
            _roleMenuService = roleMenuService;
        }


        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateRoleMenuDto dto)
        {
            await _roleMenuService.InsertAsync(dto);

            return Ok(new
            {
                message = "Menu assigned to role successfully."
            });
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _roleMenuService.GetAllAsync();

            return Ok(result);
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result =
                await _roleMenuService.GetByIdAsync(id);

            if (result == null)
            {
                return NotFound(new
                {
                    message = "Role menu not found."
                });
            }

            return Ok(result);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateRoleMenuDto dto)
        {
            await _roleMenuService.UpdateAsync(id, dto);

            return Ok(new
            {
                message = "Role menu updated successfully."
            });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _roleMenuService.DeleteAsync(id);

            return Ok(new
            {
                message = "Menu removed from role successfully."
            });
        }

        [HttpPut("restore/{id}")]
        public async Task<IActionResult> Restore(int id)
        {
            await _roleMenuService.RestoreAsync(id);

            return Ok(new
            {
                message = "Role menu restored successfully."
            });
        }

        [HttpGet("get-menus-by-role/{roleId}")]
        public async Task<IActionResult> GetMenusByRole(
            string roleId)
        {
            var result =
                await _roleMenuService.GetMenusByRoleAsync(roleId);

            return Ok(result);
        }
    }
}