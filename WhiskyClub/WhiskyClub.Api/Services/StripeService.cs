using Stripe;
using Stripe.Climate;
using WhiskyClub.Api.Services.Interfaces;

namespace WhiskyClub.Api.Services;

public class StripeService : IStripeService
{
    private readonly CustomerService _customerService;
    private readonly SubscriptionService _subscriptionService;

    public StripeService(IConfiguration configuration)
    {
        StripeConfiguration.ApiKey = configuration["Stripe:Secret"];

        _customerService = new CustomerService();
        _subscriptionService = new SubscriptionService();
    }

    public Task<Customer> CreateCustomerAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<Subscription> CreateSubscriptionAsync(string email)
    {
        throw new NotImplementedException();
    }
}