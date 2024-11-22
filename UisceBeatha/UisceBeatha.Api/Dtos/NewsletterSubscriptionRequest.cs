using System.ComponentModel.DataAnnotations;

namespace UisceBeatha.Api.Dtos;

public class NewsletterSubscriptionRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; init; } = string.Empty;
}