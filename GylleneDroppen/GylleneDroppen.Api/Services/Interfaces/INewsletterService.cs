using GylleneDroppen.Api.Utilities;

namespace GylleneDroppen.Api.Services.Interfaces;

public interface INewsletterService
{
    Task<ServiceResponse<string>> SendConfirmationEmailAsync(string email);
    Task<ServiceResponse<string>> ConfirmSubscriptionAsync(string email, string token);
}