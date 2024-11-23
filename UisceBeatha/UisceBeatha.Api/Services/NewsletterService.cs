using UisceBeatha.Api.Exceptions;
using UisceBeatha.Api.Models;
using UisceBeatha.Api.Repositories.Interfaces;
using UisceBeatha.Api.Services.Interfaces;
using UisceBeatha.Api.Utilities;

namespace UisceBeatha.Api.Services;

public class NewsletterService(
    IConfiguration configuration,
    IRedisRepository redisRepository,
    INewsletterRepository newsletterRepository,
    ISmtpService smtpService)
    : INewsletterService
{
    private readonly string _redirectUrl = configuration["NewsletterSettings:ConfirmRedirectUrl"] ?? throw new NullReferenceException("Please set NewsletterSettings:ConfirmRedirectUrl");
    private readonly string _invalidRedirectUrl = configuration["NewsletterSettings:InvalidTokenRedirectUrl"] ?? throw new NullReferenceException("Please set NewsletterSettings:InvalidTokenRedirectUrl");
    private readonly string _baseUrl = configuration["BaseUrl"] ?? throw new NullReferenceException("Please set BaseUrl");

    public async Task<string> SendConfirmationEmailAsync(string email)
    {
        var emailConfirmed = await newsletterRepository.FindAsync(s => s.Email == email);
        if (emailConfirmed.Any())
        {
            throw new EmailAlreadyConfirmedException(email);
        }
        
        var existingToken = await redisRepository.GetAsync(email.ToLowerInvariant(), "newsletter:confirm");
        if (existingToken != null)
        {
            throw new ConfirmationLinkAlreadySentException();
        }
        
        var token = TokenGenerator.GenerateToken();

        var confirmationLink = $"{_baseUrl}/api/newsletter/confirm?email={Uri.EscapeDataString(email)}&token={Uri.EscapeDataString(token)}";

        await smtpService.SendEmailAsync(
            fromEmail: "info@gyllenedroppen.se",
            toEmail: email,
            subject: "New Newsletter confirmation",
            message: $"""
                          <div style="font-family: Arial, sans-serif; text-align: center;">
                              <h1 style="color: #6A4A3C;">Tack för din intresseanmälan!</h1>
                              <p>Vi är glada över att du vill bli en del av vår whisky-gemenskap.</p>
                              <p>Bekräfta din prenumeration genom att klicka på länken nedan:</p>
                              <a 
                                  href='{confirmationLink}' 
                                  style="display: inline-block; margin: 20px 0; padding: 10px 20px; background-color: #C19A6B; color: #fff; text-decoration: none; border-radius: 5px; font-weight: bold;">
                                  Bekräfta prenumeration
                              </a>
                              <p>Om du inte har registrerat dig för vårt nyhetsbrev, vänligen ignorera detta e-postmeddelande.</p>
                              <footer style="margin-top: 20px; font-size: 12px; color: #888;">
                                  &copy; {DateTime.UtcNow.Year} Gyllene Droppen - Alla rättigheter förbehållna.
                              </footer>
                          </div>
                      """

        );

        await redisRepository.SaveAsync(email.ToLowerInvariant(), token, TimeSpan.FromDays(14), "newsletter:confirm");
        
        return "A confirmation email has been sent. Please check your inbox.";
    }

    public async Task<string> ConfirmSubscriptionAsync(string email, string token)
    {
        var storedToken = await redisRepository.GetAsync(email.ToLowerInvariant(), "newsletter:confirm");
        if (storedToken == null || storedToken != token)
        {
            return _invalidRedirectUrl;
        }
        
        await redisRepository.DeleteAsync(email.ToLowerInvariant(), "newsletter:confirm");

        var subscription = new NewsletterSubscription
        {
            Id = Guid.NewGuid(),
            Email = email,
            SubscribedAt = DateTime.UtcNow
        };
        
        await newsletterRepository.AddAsync(subscription);
        if(!await newsletterRepository.SaveChangesAsync())
            throw new Exception("Failed to add new subscription");

        return _redirectUrl;
    }
}