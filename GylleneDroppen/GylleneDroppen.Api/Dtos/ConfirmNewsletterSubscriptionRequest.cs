namespace GylleneDroppen.Api.Dtos;

public class ConfirmNewsletterSubscriptionRequest
{
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}