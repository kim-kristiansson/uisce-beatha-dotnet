using Microsoft.AspNetCore.Mvc;
using UisceBeatha.Api.Dtos;
using UisceBeatha.Api.Services.Interfaces;

namespace UisceBeatha.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController(IEmailService emailService) :Controller
    {
        [HttpGet]
        public async Task<IActionResult> SendEmail()
        {
            await emailService.SendEmailAsync();
                
            return Ok();
            
        }
    }
}