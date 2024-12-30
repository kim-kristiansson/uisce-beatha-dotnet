namespace GylleneDroppen.Api.Dtos;

public class RegisterResponse
{
    public string Email { get; init; } = string.Empty;
    public string StripeSessionId { get; init; } = string.Empty;
}