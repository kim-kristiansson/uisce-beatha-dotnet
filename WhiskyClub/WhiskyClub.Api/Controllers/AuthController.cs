using Microsoft.AspNetCore.Mvc;
using WhiskyClub.Api.Dtos;
using WhiskyClub.Api.Services.Interfaces;

namespace WhiskyClub.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService) :Controller
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request) => Ok(await authService.RegisterAsync(request));

    }
}