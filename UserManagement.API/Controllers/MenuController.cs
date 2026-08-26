using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.DTOs.Menu;
using UserManagement.Application.Interfaces;

namespace UserManagement.API.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Super User")]
    [ApiController]
    [Route("api/menu")]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateMenuDto dto)
        {
            await _menuService.InsertAsync(dto);

            return StatusCode(
                StatusCodes.Status201Created,
                new
                {
                    statusCode = StatusCodes.Status201Created,
                    message = "Menu created successfully."
                });
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _menuService.GetAllAsync();

            return Ok(new
            {
                statusCode = StatusCodes.Status200OK,
                message = "Menus retrieved successfully.",
                data = result
            });
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _menuService.GetByIdAsync(id);

            return Ok(new
            {
                statusCode = StatusCodes.Status200OK,
                message = "Menu retrieved successfully.",
                data = result
            });
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id,UpdateMenuDto dto)
        {
            await _menuService.UpdateAsync(id, dto);

            return Ok(new
            {
                statusCode = StatusCodes.Status200OK,
                message = "Menu updated successfully."
            });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _menuService.DeleteAsync(id);

            return Ok(new
            {
                statusCode = StatusCodes.Status200OK,
                message = "Menu deleted successfully."
            });
        }

        [HttpPut("restore/{id}")]
        public async Task<IActionResult> Restore(int id)
        {
            await _menuService.RestoreAsync(id);

            return Ok(new
            {
                statusCode = StatusCodes.Status200OK,
                message = "Menu restored successfully."
            });
        }
    }
}