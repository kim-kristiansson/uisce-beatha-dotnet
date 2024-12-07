using GylleneDroppen.Api.Configurations;
using GylleneDroppen.Api.Models;
using GylleneDroppen.Api.Repositories.Interfaces;
using GylleneDroppen.Api.Services.Interfaces;
using GylleneDroppen.Api.Utilities;
using Microsoft.Extensions.Options;

namespace GylleneDroppen.Api.Services;

public class NewsletterService(
    IRedisRepository redisRepository,
    INewsletterRepository newsletterRepository,
    ISmtpService smtpService,
    IAnalyticsService analyticsService,
    IOptions<NewsletterConfig> newsletterConfigOptions,
    IOptions<GlobalConfig> globalConfigOptions,
    IOptions<FrontendConfig> frontendConfigOptions)
    : INewsletterService
{
    private readonly NewsletterConfig _newsletterConfig = newsletterConfigOptions.Value;
    private readonly FrontendConfig _frontendConfig = frontendConfigOptions.Value;
    private readonly string _baseUrl = globalConfigOptions.Value.BaseUrl;

    public async Task<ServiceResponse<string>> SendConfirmationEmailAsync(string email)
    {
        var emailConfirmed = await newsletterRepository.FindAsync(s => s.Email == email);
        if (emailConfirmed.Any())
            return ServiceResponse<string>.Failure(
                title: "Email already in use",
                detail: $"The email {email} is already in use.",
                statusCode: StatusCodes.Status409Conflict
            );

        var existingToken = await redisRepository.GetAsync(email.ToLowerInvariant(), "newsletter:confirm");
        if (existingToken != null)
            return ServiceResponse<string>.Failure(
                title: "Confirmation mail already sent",
                detail: $"A confirmation mail has already been sent to {email}.",
                statusCode: StatusCodes.Status409Conflict
            );
        
        var token = TokenGenerator.GenerateToken();

        var confirmationLink = $"{_baseUrl}/api/newsletter/confirm?email={Uri.EscapeDataString(email)}&token={Uri.EscapeDataString(token)}";

        await smtpService.SendEmailAsync(
            displayName: "Gyllene Droppen",
            emailAlias: "noreply",
            toEmail: email,
            subject: "Bekräfta din e-post för vårt nyhetsbrev",
            message: $"""
                          <div style="font-family: Arial, sans-serif; text-align: center;">
                              <img src="https://gyllenedroppen.se/Images/gyllene-droppen.png" alt="Gyllene Droppen" style="width: 150px; height: auto; margin-bottom: 20px;">
                              <h1 style="color: #6A4A3C;">Tack för din intresseanmälan!</h1>
                              <p>Vi är glada över att du vill bli en del av vår whisky-gemenskap.</p>
                              <p>Bekräfta din e-post genom att klicka på länken nedan:</p>
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
        
        await analyticsService.IncrementSignUpAsync();
        if(!await newsletterRepository.SaveChangesAsync())
            return ServiceResponse<string>.Failure(
                title: "Failed to save analytics.",
                detail: $"Something went wrong. The server couldn't save analytics.",
                statusCode: StatusCodes.Status500InternalServerError
            );
        
        await redisRepository.SaveAsync(email.ToLowerInvariant(), token, TimeSpan.FromHours(24), "newsletter:confirm");
        
        return ServiceResponse<string>.Success("A confirmation email has been sent. Please check your inbox.");
    }

    public async Task<ServiceResponse<string>> ConfirmSubscriptionAsync(string email, string token)
    {
        var storedToken = await redisRepository.GetAsync(email.ToLowerInvariant(), "newsletter:confirm");
        if (storedToken == null || storedToken != token)
            return ServiceResponse<string>.Redirect(_frontendConfig.BaseUrl + _frontendConfig.Paths.Error);
        
        
        await redisRepository.DeleteAsync(email.ToLowerInvariant(), "newsletter:confirm");

        var subscription = new NewsletterSubscription
        {
            Id = Guid.NewGuid(),
            Email = email,
            SubscribedAt = DateTime.UtcNow
        };
        
        await newsletterRepository.AddAsync(subscription);
        await analyticsService.IncrementConfirmedSignUpAsync();
        
        if(!await newsletterRepository.SaveChangesAsync())
            return ServiceResponse<string>.Redirect(_frontendConfig.BaseUrl + _frontendConfig.Paths.Error);

        return ServiceResponse<string>.Redirect(_frontendConfig.BaseUrl + _frontendConfig.Paths.Success);
    }
}