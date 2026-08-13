using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.DTOs.User;
using UserManagement.Application.Interfaces;
using UserManagement.Infrastructure.Services;

namespace UserManagement.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Super User")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPut("change-status")]
        public async Task<IActionResult> ChangeUserStatus([FromBody] ChangeUserStatusDto dto)
        {
            var result =
                await _userService.ChangeUserStatusAsync(dto);

            return Ok(new
            {
                message = result
            });
        }


        [HttpGet("get-all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAllUsersAsync();

            return Ok(result);
        }


        [HttpGet("get-user/{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var result = await _userService.GetUserByIdAsync(userId);

            return Ok(result);
        }

        [HttpPut("update-user/{userId}")]
        public async Task<IActionResult> UpdateUser(string userId,[FromBody] UpdateUserDto dto)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var result = await _userService.UpdateUserAsync(userId, dto);

            return Ok(new
            {
                message = "User updated successfully."
            });
        }


        [HttpDelete("delete-user/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest(new
                {
                    message = "User ID is required."
                });
            }

            var result =
                await _userService.DeleteUserAsync(userId);

            return Ok(new
            {
                message = "User deleted successfully."
            });
        }

    }
}
