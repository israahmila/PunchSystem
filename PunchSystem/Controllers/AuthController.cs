using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PunchSystem.Contracts;
using PunchSystem.DTOs;
using PunchSystem.Security;

namespace PunchSystem.Controllers
{
        [ApiController]
        [Route("api/[controller]")]
        public class AuthController : ControllerBase
        {
            private readonly IAuthService _authService;
            private readonly IHttpContextAccessor _http;

            public AuthController(IAuthService authService, IHttpContextAccessor http)
            {
                _authService = authService;
                _http = http;
            }

            [HttpPost("register")]
            public async Task<IActionResult> Register(RegisterRequest request)
            {
                var success = await _authService.RegisterAsync(request);
                return success ? Ok("User registered") : BadRequest("Username already exists");
            }

            [HttpPost("login")]
            public async Task<IActionResult> Login(LoginRequest request)
            {
                var ip = _http.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "unknown";
                try
                {
                    var result = await _authService.LoginAsync(request, ip);
                    return Ok(result);
                }
                catch (UnauthorizedAccessException ex)
                {
                    return Unauthorized(ex.Message);
                }
            }

            [Authorize]
            [HasPermission("EditUsers")]
            [HttpPut("users/{id}")]
            public IActionResult EditUser(int id)
            {
                return Ok("You have access");
            }
        }
    }

