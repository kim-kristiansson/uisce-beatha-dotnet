namespace GylleneDroppen.Api.Services.Interfaces;

public interface ISmtpService
{
    Task SendEmailAsync(string displayName, string fromEmail, string toEmail, string subject, string message);
}