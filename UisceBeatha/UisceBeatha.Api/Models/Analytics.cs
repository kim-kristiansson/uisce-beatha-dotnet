namespace UisceBeatha.Api.Models;

public class Analytics
{
    public Guid Id { get; set; }
    public int NewsletterSignUps { get; set; } = 0;
    public int ConfirmedNewsletterSignUps { get; set; } = 0;
    public DateTime LastUpdate { get; set; }
}