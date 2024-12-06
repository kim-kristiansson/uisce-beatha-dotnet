namespace GylleneDroppen.Api.Services.Interfaces;

public interface ISmtpService
{
    Task SendEmailAsync(string displayName, string emailAlias, string toEmail, string subject, string message);
}