using GylleneDroppen.Api.Dtos;
using GylleneDroppen.Api.Extensions;
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
        public async Task<IActionResult> SendEmail([FromBody] NewsletterSubscriptionRequest request)
        {
            var response = await newsletterService.SendConfirmationEmailAsync(request.Email);

            return response.ToActionResult();
        }

        [HttpGet("confirm")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmNewsletterSubscriptionRequest request)
        {
            var response = await newsletterService.ConfirmSubscriptionAsync(request.Email, request.Token);
            
            return response.ToActionResult();
        }
    }
}