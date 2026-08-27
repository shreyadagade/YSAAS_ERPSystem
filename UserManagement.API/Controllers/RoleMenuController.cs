using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.DTOs.RoleMenu;
using UserManagement.Application.Interfaces;

namespace UserManagement.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "Super User")]
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
        public async Task<IActionResult> Create(CreateRoleMenuDto dto)
        {
            var result = await _roleMenuService.InsertAsync(dto);

            return StatusCode(
                StatusCodes.Status201Created,
                new
                {
                    statusCode = StatusCodes.Status201Created,
                    message = "Menu assigned to role successfully."
                });
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _roleMenuService.GetAllAsync();

            return StatusCode(
                StatusCodes.Status200OK,
                new
                {
                    statusCode = StatusCodes.Status200OK,
                    message = "Role menus retrieved successfully.",
                    data = result
                });
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _roleMenuService.GetByIdAsync(id);

            return StatusCode(
                StatusCodes.Status200OK,
                new
                {
                    statusCode = StatusCodes.Status200OK,
                    message = "Role menu retrieved successfully.",
                    data = result
                });
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id,UpdateRoleMenuDto dto)
        {
            var result = await _roleMenuService.UpdateAsync(id, dto);

            return StatusCode(
                StatusCodes.Status200OK,
                new
                {
                    statusCode = StatusCodes.Status200OK,
                    message = "Role menu updated successfully."
                });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _roleMenuService.DeleteAsync(id);

            return StatusCode(
                StatusCodes.Status200OK,
                new
                {
                    statusCode = StatusCodes.Status200OK,
                    message = "Menu removed from role successfully."
                });
        }

        [HttpPut("restore/{id}")]
        public async Task<IActionResult> Restore(int id)
        {
            var result = await _roleMenuService.RestoreAsync(id);

            return StatusCode(
                StatusCodes.Status200OK,
                new
                {
                    statusCode = StatusCodes.Status200OK,
                    message = "Role menu restored successfully."
                });
        }

        [HttpGet("get-menus-by-role/{roleId}")]
        public async Task<IActionResult> GetMenusByRole(string roleId)
        {
            var result = await _roleMenuService.GetMenusByRoleAsync(roleId);

            return StatusCode(
                StatusCodes.Status200OK,
                new
                {
                    statusCode = StatusCodes.Status200OK,
                    message = "Menus retrieved successfully.",
                    data = result
                });
        }
    }
}