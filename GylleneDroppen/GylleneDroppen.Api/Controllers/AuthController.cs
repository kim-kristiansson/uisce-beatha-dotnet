using Microsoft.AspNetCore.Mvc;
using GylleneDroppen.Api.Dtos;
using GylleneDroppen.Api.Extensions;
using GylleneDroppen.Api.Services.Interfaces;

namespace GylleneDroppen.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService) :Controller
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var response = await authService.RegisterAsync(request);
            return response.ToActionResult();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await authService.LoginAsync(request);
            return response.ToActionResult();
        }
    }
}