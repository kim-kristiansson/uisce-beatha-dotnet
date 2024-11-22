namespace UisceBeatha.Api.Services.Interfaces;

public interface ISmtpService
{
    Task SendEmailAsync(string fromEmail, string toEmail, string subject, string message);
}