// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using GylleneDroppen.Api.Dtos;
// using GylleneDroppen.Api.Services.Interfaces;
//
// namespace GylleneDroppen.Api.Controllers;
//
// [Authorize]
// [ApiController]
// [Route("api/[controller]")]
// public class StripeController(IStripeService stripeService) : ControllerBase
// {
//     [HttpPost("subscribe")]
//     public async Task<IActionResult> Subscribe([FromBody] StripeSubscriptionRequest request)
//         => Ok(await stripeService.EnsureSubscription(request));
// }