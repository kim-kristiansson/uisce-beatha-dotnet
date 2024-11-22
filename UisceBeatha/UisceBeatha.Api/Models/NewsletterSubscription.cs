namespace UisceBeatha.Api.Models;

public class NewsletterSubscription
{
    public Guid Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public DateTime SubscribedAt { get; init; }
}