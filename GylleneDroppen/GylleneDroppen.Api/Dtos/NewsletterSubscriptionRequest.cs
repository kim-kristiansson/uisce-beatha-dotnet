using System.ComponentModel.DataAnnotations;

namespace GylleneDroppen.Api.Dtos;

public class NewsletterSubscriptionRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; init; } = string.Empty;
}