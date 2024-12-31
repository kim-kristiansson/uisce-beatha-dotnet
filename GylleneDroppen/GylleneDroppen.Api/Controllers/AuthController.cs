﻿using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest request)
        {
            var response = await authService.VerifyEmailAsync(request);
            return response.ToActionResult();
        }
    }
}