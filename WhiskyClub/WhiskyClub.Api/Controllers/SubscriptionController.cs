using Microsoft.AspNetCore.Mvc;
using WhiskyClub.Api.Dtos;
using WhiskyClub.Api.Services;

namespace WhiskyClub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubscriptionController(StripeService stripeService) : ControllerBase
{
    [HttpPost("subscribe")]
    public async Task<IActionResult> Subscribe([FromBody] SubscriptionRequestDto subscriptionRequestDto)
        => Ok(await stripeService.CreateSubscriptionAsync(subscriptionRequestDto.Email));
}