using GylleneDroppen.Api.Configurations;
using GylleneDroppen.Api.Dtos;
using GylleneDroppen.Api.Repositories.Interfaces;
using GylleneDroppen.Api.Services.Interfaces;
using Microsoft.Extensions.Options;
using Stripe;

namespace GylleneDroppen.Api.Services;

public class StripeService(IOptions<StripeConfig> stripeConfigOptions, IUserRepository userRepository)
    : IStripeService
{
    private readonly CustomerService _customerService = new();
    private readonly SubscriptionService _subscriptionService = new();
    private readonly StripeConfig _stripeConfig = stripeConfigOptions.Value;

    public async Task<Subscription> EnsureSubscription(StripeSubscriptionRequest request)
    {
        var user = await userRepository.GetByIdAsync(request.UserId);

        if (user == null)
        {
            throw new Exception($"User with id {request.UserId} not found");
        }

        if (!string.IsNullOrEmpty(user.StripeCustomerId)) 
            return await CreateSubscriptionAsync(user.StripeCustomerId, request.PriceId);
        
        user.StripeCustomerId = await CreateCustomerAsync(user.Email);
        userRepository.Update(user);
        
        if(!await userRepository.SaveChangesAsync())
            throw new Exception($"Internal error updating user with id {user.Id}");

        return await CreateSubscriptionAsync(user.StripeCustomerId, request.PriceId);
    }

    private async Task<string> CreateCustomerAsync(string email)
    {
        var customerOptions = new CustomerCreateOptions
        {
            Email = email
        };
        
        return (await _customerService.CreateAsync(customerOptions)).Id;
    }

    private async Task<Subscription> CreateSubscriptionAsync(string customerId, string priceId)
    {
        return await _subscriptionService.CreateAsync(new SubscriptionCreateOptions
        {
            Customer = customerId,
            Items =
            [
                new SubscriptionItemOptions
                {
                    Price = priceId
                }
            ]
        });
    }
}