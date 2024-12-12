using GylleneDroppen.Api.Dtos;
using GylleneDroppen.Api.Utilities;

namespace GylleneDroppen.Api.Services.Interfaces;

public interface INewsletterService
{
    Task<ServiceResponse<SentNewsletterConfirmEmailResponse>> SendConfirmationEmailAsync(string email);
    Task<ServiceResponse<string>> ConfirmSubscriptionAsync(string email, string token);
}