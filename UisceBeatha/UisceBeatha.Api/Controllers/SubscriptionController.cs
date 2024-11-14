using Microsoft.AspNetCore.Mvc;
using UisceBeatha.Api.Dtos;
using UisceBeatha.Api.Services;

namespace UisceBeatha.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubscriptionController(StripeService stripeService) : ControllerBase
{
    [HttpPost("subscribe")]
    public async Task<IActionResult> Subscribe([FromBody] SubscriptionRequestDto subscriptionRequestDto)
        => Ok(await stripeService.CreateSubscriptionAsync(subscriptionRequestDto.Email));
}