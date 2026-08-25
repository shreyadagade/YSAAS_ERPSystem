using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.DTOs.Menu;
using UserManagement.Application.Interfaces;

namespace UserManagement.API.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "Super User")]
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
        public async Task<IActionResult> Create([FromBody] CreateMenuDto dto)
        {
            var result =
                await _menuService.InsertAsync(dto);

            return StatusCode(
                StatusCodes.Status201Created,
                new
                {
                    message = "Menu created successfully."
                });
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result =
                await _menuService.GetAllAsync();

            return Ok(result);
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result =
                await _menuService.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id,[FromBody] UpdateMenuDto dto)
        {
            var result =
                await _menuService.UpdateAsync(id, dto);

            return Ok(new
            {
                message = "Menu updated successfully."
            });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result =
                await _menuService.DeleteAsync(id);

            return Ok(new
            {
                message = "Menu deleted successfully."
            });
        }

        [HttpPut("restore/{id}")]
        public async Task<IActionResult> Restore(int id)
        {
            var result =
                await _menuService.RestoreAsync(id);

            return Ok(new
            {
                message = "Menu restored successfully."
            });
        }
    }
}