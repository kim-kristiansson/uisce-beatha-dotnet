using System.ComponentModel.DataAnnotations;

namespace UisceBeatha.Api.Dtos;

public class StripeSubscriptionRequest
{
    [Required]
    public Guid UserId { get; init; }
    
    [Required]
    public string Email { get; init; } = string.Empty;
    
    [Required]
    public string PriceId { get; init; } = string.Empty;
}