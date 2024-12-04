using GylleneDroppen.Api.Models;
using GylleneDroppen.Api.Repositories.Interfaces;
using GylleneDroppen.Api.Services.Interfaces;

namespace GylleneDroppen.Api.Services;

public class AnalyticsService(IAnalyticsRepository analyticsRepository) : IAnalyticsService
{
    
    private async Task<Analytics> GetOrCreateAsync()
    {
        var analytics = (await analyticsRepository.GetAllAsync()).FirstOrDefault();

        if (analytics is not null) return analytics;
        
        analytics = new Analytics();
        await analyticsRepository.AddAsync(analytics);
        if(!await analyticsRepository.SaveChangesAsync())
            throw new Exception("An error occured while adding analytics");
        
        return analytics;
    }
    
    public async Task IncrementSignUpAsync()
    {
        var analytics = await GetOrCreateAsync();
        analytics.NewsletterSignUps++;
        analytics.LastUpdate = DateTime.UtcNow;
        analyticsRepository.Update(analytics);
    }

    public async Task IncrementConfirmedSignUpAsync()
    {
        var analytics = await GetOrCreateAsync();
        analytics.ConfirmedNewsletterSignUps++;
        analytics.LastUpdate = DateTime.UtcNow;
        analyticsRepository.Update(analytics);
    }

    public async Task<int> GetSignUpCountAsync()
    {
        var analytics = await GetOrCreateAsync();
        return analytics.NewsletterSignUps;
    }
}