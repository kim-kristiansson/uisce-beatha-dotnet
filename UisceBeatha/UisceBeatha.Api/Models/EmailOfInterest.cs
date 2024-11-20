namespace UisceBeatha.Api.Models;

public class EmailOfInterest
{
    public Guid Id { get; set; }
    public string? EmailAddres { get; init; } 
    public DateTime CreatedAt { get; set; }
}