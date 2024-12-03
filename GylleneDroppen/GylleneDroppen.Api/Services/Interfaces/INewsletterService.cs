namespace GylleneDroppen.Api.Services.Interfaces;

public interface INewsletterService
{
    Task<string> SendConfirmationEmailAsync(string email);
    Task<string> ConfirmSubscriptionAsync(string email, string token);
}