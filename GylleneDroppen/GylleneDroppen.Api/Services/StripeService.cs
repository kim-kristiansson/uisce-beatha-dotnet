using GylleneDroppen.Api.Dtos;
using GylleneDroppen.Api.Repositories.Interfaces;
using GylleneDroppen.Api.Services.Interfaces;
using Stripe;
using Stripe.Climate;

namespace GylleneDroppen.Api.Services;

public class StripeService : IStripeService
{
    private readonly IUserRepository _userRepository;
    private readonly CustomerService _customerService;
    private readonly SubscriptionService _subscriptionService;

    public StripeService(IConfiguration configuration, IUserRepository userRepository)
    {
        StripeConfiguration.ApiKey = configuration["Stripe:Secret"];

        _userRepository = userRepository;
        _customerService = new CustomerService();
        _subscriptionService = new SubscriptionService();
    }

    public async Task<Subscription> EnsureSubscription(StripeSubscriptionRequest request)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);

        if (user == null)
        {
            throw new Exception($"User with id {request.UserId} not found");
        }

        if (!string.IsNullOrEmpty(user.StripeCustomerId)) 
            return await CreateSubscriptionAsync(user.StripeCustomerId, request.PriceId);
        
        user.StripeCustomerId = await CreateCustomerAsync(user.Email);
        _userRepository.Update(user);
        
        if(!await _userRepository.SaveChangesAsync())
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