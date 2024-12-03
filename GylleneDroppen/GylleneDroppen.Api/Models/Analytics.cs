namespace GylleneDroppen.Api.Models;

public class Analytics
{
    public Guid Id { get; init; }
    public int NewsletterSignUps { get; set; } = 0;
    public int ConfirmedNewsletterSignUps { get; set; } = 0;
    public DateTime LastUpdate { get; set; }
}