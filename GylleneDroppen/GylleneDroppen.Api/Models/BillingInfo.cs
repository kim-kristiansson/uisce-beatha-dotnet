namespace GylleneDroppen.Api.Models;

public class BillingInfo
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string StreetAddress { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = "Sweden";
}