using Microsoft.AspNetCore.Mvc;
using UisceBeatha.Api.Dtos;
using UisceBeatha.Api.Services.Interfaces;

namespace UisceBeatha.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService) :Controller
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request) => 
            Ok(await authService.RegisterAsync(request));

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request) => 
            Ok(await authService.LoginAsync(request));
    }
}