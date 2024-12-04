using GylleneDroppen.Api.Dtos;
using GylleneDroppen.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace GylleneDroppen.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsletterController(INewsletterService newsletterService) :Controller
    {
        [EnableRateLimiting("SlidingWindowPolicy")]
        [HttpPost("register")]
        public async Task<IActionResult> SendEmail([FromBody] NewsletterSubscriptionRequest request) =>
            Ok(await newsletterService.SendConfirmationEmailAsync(request.Email));
        
        [HttpGet("confirm")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmNewsletterSubscriptionRequest request) =>
            Redirect(await newsletterService.ConfirmSubscriptionAsync(request.Email, request.Token));
    }
}