using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserManagement.Application.DTOs.User;
using UserManagement.Application.Interfaces;
using UserManagement.Infrastructure.Services;

namespace UserManagement.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;

        public UserProfileController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService                ;
        }

        [HttpGet("get-profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var profile =
                await _userProfileService.GetProfileAsync(userId);

            return Ok(new
            {
                statusCode = StatusCodes.Status200OK,
                message = "User profile retrieved successfully.",
                data = profile
            });
        }

        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var message = await _userProfileService.UpdateProfileAsync(userId,dto);

            return Ok(new
            {
                statusCode = StatusCodes.Status200OK,
                message = message
            });
        }
    }
}