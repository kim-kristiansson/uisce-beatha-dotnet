using Stripe;

namespace UisceBeatha.Api.Services.Interfaces;

public interface IStripeService
{
    Task<Customer>CreateCustomerAsync(string email);
    Task<Subscription>CreateSubscriptionAsync(string email);
}