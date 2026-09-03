using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
        public async Task<IActionResult> ChangeUserStatus(ChangeUserStatusDto dto)
        {
            var result = await _userService.ChangeUserStatusAsync(dto);

            return Ok(new
            {
                statusCode = StatusCodes.Status200OK,
                message = result
            });
        }


        [HttpGet("get-all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAllUsersAsync();

            return Ok(new
            {
                statusCode = StatusCodes.Status200OK,
                message = "Users retrieved successfully.",
                data = result
            });
        }

        [HttpGet("get-user/{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var result = await _userService.GetUserByIdAsync(userId);

            return Ok(new
            {
                statusCode = StatusCodes.Status200OK,
                message = "User retrieved successfully.",
                data = result
            });
        }

        [HttpPut("update-user/{userId}")]
        public async Task<IActionResult> UpdateUser(string userId,UpdateUserDto dto)
        {
            var result = await _userService.UpdateUserAsync(userId, dto);

            return Ok(new
            {
                statusCode = StatusCodes.Status200OK,
                message = result
            });
        }


        [HttpDelete("delete-user/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var result = await _userService.DeleteUserAsync(userId);

            return Ok(new
            {
                statusCode = StatusCodes.Status200OK,
                message = "User deleted successfully."
            });
        }

        

    }
}
