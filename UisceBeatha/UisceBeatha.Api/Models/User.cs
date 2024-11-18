namespace UisceBeatha.Api.Models
{
    public class User
    {
        public Guid Id { get; init; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string StripeCustomerId { get; set; } = string.Empty;
    }
}
