using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UisceBeatha.Api.Dtos;
using UisceBeatha.Api.Services.Interfaces;

namespace UisceBeatha.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class StripeController(IStripeService stripeService) : ControllerBase
{
    // [HttpPost("subscribe")]
    // public async Task<IActionResult> Subscribe([FromBody] StripeSubscriptionRequest request)
    //     => Ok(await stripeService.EnsureSubscription(request));
}