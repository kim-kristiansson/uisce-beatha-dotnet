using GylleneDroppen.Api.Services.Interfaces;
using Stripe;
using Stripe.Checkout;

namespace GylleneDroppen.Api.Services;

public class StripeService : IStripeService
{
    private readonly IConfiguration _configuration;

    public StripeService(IConfiguration configuration)
    {
        _configuration = configuration;
        StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
    }
    
    public async Task<string> CreateCheckoutSessionAsync(string email, string clientReferenceId, string successUrl, string cancelUrl)
    {
        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = [ "card" ],
            Mode = "subscription",
            CustomerEmail = email,
            ClientReferenceId = clientReferenceId,
            LineItems =
            [
                new SessionLineItemOptions
                {
                    Price = _configuration["Stripe:PriceId"],
                    Quantity = 1
                }
            ],
            SuccessUrl = successUrl,
            CancelUrl = cancelUrl
        };
        
        var service = new SessionService();
        var session = await service.CreateAsync(options);

        return session.Id;
    }

    public async Task<bool> ValidateWebhookAsync(HttpRequest request)
    {
        var json = await new StreamReader(request.Body).ReadToEndAsync();
        var endpointSecret = _configuration["Stripe:WebhookSecret"];

        try
        {
            var stripeEvent = EventUtility.ConstructEvent(
                json,
                request.Headers["Stripe-Signature"],
                endpointSecret
            );
            
            return stripeEvent != null;
        }
        catch (StripeException)
        {
            return false;
        }
    }

    public async Task<Session> GetSessionDetailsAsync(string sessionId)
    {
        var service = new SessionService();
        return await service.GetAsync(sessionId);
    }
}