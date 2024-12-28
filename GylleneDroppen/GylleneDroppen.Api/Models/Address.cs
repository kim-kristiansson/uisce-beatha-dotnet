namespace GylleneDroppen.Api.Models;

public class Address
{
    public Guid Id { get; init; }
    public string StreetAddress { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}