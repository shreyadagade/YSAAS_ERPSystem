using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Application.DTOs.Login;
using StudentManagement.Application.Interfaces.Services.Login;


namespace StudentManagement.API.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class StudentLoginController : ControllerBase
        {
            private readonly IStudentLoginService _loginService;

            public StudentLoginController(
                IStudentLoginService loginService)
            {
                _loginService = loginService;
            }

            // =====================================================
            // STUDENT LOGIN
            // =====================================================
            [HttpPost("login")]
            public async Task<IActionResult> Login(
                StudentLoginRequestDto request)
            {
                var result =
                    await _loginService.LoginAsync(request);

                return Ok(result);
            }
        }
    }
