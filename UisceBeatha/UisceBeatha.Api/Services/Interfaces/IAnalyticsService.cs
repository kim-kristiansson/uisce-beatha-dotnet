namespace UisceBeatha.Api.Services.Interfaces;

public interface IAnalyticsService
{
    Task IncrementSignUpAsync();
    Task IncrementConfirmedSignUpAsync();
    Task<int> GetSignUpCountAsync();
}