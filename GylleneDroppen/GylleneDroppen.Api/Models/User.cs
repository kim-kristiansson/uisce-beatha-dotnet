namespace GylleneDroppen.Api.Models
{
    public class User
    {
        public Guid Id { get; init; }
        public string Email { get; init; } = string.Empty;
        public string PasswordHash { get; init; } = string.Empty;
        public string Firstname { get; init; } = string.Empty;
        public string Lastname { get; init; } = string.Empty;

        public Address Address { get; init; } = null!;
    }
}
