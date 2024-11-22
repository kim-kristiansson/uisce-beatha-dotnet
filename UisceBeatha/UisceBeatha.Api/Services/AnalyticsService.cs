using UisceBeatha.Api.Models;
using UisceBeatha.Api.Repositories.Interfaces;
using UisceBeatha.Api.Services.Interfaces;

namespace UisceBeatha.Api.Services;

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
        analyticsRepository.Update(analytics);
        if(!await analyticsRepository.SaveChangesAsync())
            throw new Exception("An error occured while updating analytics");
    }

    public async Task IncrementConfirmedSignUpAsync()
    {
        var analytics = await GetOrCreateAsync();
        analytics.ConfirmedNewsletterSignUps++;
        analyticsRepository.Update(analytics);
        if(!await analyticsRepository.SaveChangesAsync())
            throw new Exception("An error occured while updating analytics");
    }

    public async Task<int> GetSignUpCountAsync()
    {
        var analytics = await GetOrCreateAsync();
        return analytics.NewsletterSignUps;
    }
}