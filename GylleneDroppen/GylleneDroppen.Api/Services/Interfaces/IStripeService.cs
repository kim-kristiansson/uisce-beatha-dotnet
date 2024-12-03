using GylleneDroppen.Api.Dtos;
using Stripe;

namespace GylleneDroppen.Api.Services.Interfaces;

public interface IStripeService
{
    Task<Subscription>EnsureSubscription(StripeSubscriptionRequest request);
}