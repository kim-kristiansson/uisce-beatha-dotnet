using Stripe;
using UisceBeatha.Api.Dtos;

namespace UisceBeatha.Api.Services.Interfaces;

public interface IStripeService
{
    Task<Subscription>EnsureSubscription(StripeSubscriptionRequest request);
}