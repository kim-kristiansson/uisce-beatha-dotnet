using Stripe.Checkout;

namespace GylleneDroppen.Api.Services.Interfaces;

public interface IStripeService
{
    Task<string> CreateCheckoutSessionAsync(string email, string clientReferenceId, string successUrl, string cancelUrl);
    Task<bool> ValidateWebhookAsync(HttpRequest request);
    Task<Session> GetSessionDetailsAsync(string sessionId);
}