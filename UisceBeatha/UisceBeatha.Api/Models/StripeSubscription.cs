namespace UisceBeatha.Api.Models;

public class StripeSubscription
{
    public string? Id { get; set; } = string.Empty;
    public string? CustomerId { get; set; } = string.Empty;
    public string? Status { get; set; } = string.Empty;
    public DateTime? StartDate { get; set; } = DateTime.MinValue;
    public DateTime? EndDate { get; set; } = DateTime.MinValue;
}