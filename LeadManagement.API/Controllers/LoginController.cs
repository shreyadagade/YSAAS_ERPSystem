using LeadManagement.Application.DTOs.Login;
using LeadManagement.Application.Interfaces.Services;
using LeadManagement.Application.Interfaces.Services.Login;
using Microsoft.AspNetCore.Mvc;

namespace LeadManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public LoginController(
            ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequestDto request)
        {
            var result =
                await _loginService.LoginAsync(request);

            if (result == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid username or password."
                });
            }

            return Ok(result);
        }
    }
}