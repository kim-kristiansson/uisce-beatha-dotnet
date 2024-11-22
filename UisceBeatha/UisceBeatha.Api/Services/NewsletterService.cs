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

    public async Task<string> SendConfirmationEmailAsync(string email)
    {
        var existingToken = await redisRepository.GetAsync(email.ToLowerInvariant(), "newsletter:confirm");
        if (existingToken != null)
        {
            throw new Exception("A confirmation link has already been sent to this email. Please check your inbox.");
        }
        
        var token = TokenGenerator.GenerateToken();
        
        await redisRepository.SaveAsync(email.ToLowerInvariant(), token, TimeSpan.FromDays(14), "newsletter:confirm");

        var confirmationLink = $"https://localhost:7254/api/newsletter/confirm?email={Uri.EscapeDataString(email)}&token={Uri.EscapeDataString(token)}";

        await smtpService.SendEmailAsync(
            fromEmail: "info@gyllenedroppen.se",
            toEmail: email,
            subject: "New Newsletter confirmation",
            message: $"""
                        <p>Thank you for subscribing to our newsletter!</p>
                        <p>Please confirm your subscription by clicking the link below:</p>
                        <a href='{confirmationLink}'>Confirm Subscription</a>
                      """
        );

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