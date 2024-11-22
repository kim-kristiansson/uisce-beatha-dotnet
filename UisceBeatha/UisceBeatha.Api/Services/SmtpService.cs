using System.Net;
using System.Net.Mail;
using UisceBeatha.Api.Configurations;
using UisceBeatha.Api.Models;
using UisceBeatha.Api.Repositories.Interfaces;
using UisceBeatha.Api.Services.Interfaces;

namespace UisceBeatha.Api.Services;

public class SmtpService : ISmtpService
{
    private readonly EmailSettings _emailSettings;
    
    public SmtpService(IConfiguration configuration)
    {
        _emailSettings = configuration.GetSection("SmtpSettings").Get<EmailSettings>()
                         ?? throw new ArgumentNullException(nameof(configuration), "Email configuration section is missing or invalid.");

        if (string.IsNullOrEmpty(_emailSettings.SmtpServer) || _emailSettings.Port == 0 || _emailSettings.EmailAccounts == null || _emailSettings.EmailAccounts.Count == 0)
        {
            throw new ArgumentException("Email configuration is invalid. Ensure all required fields are set in appsettings.json.");
        }
    }
    
    public async Task SendEmailAsync(string fromEmail, string toEmail, string subject, string message)
    {
        var emailAccount = _emailSettings.EmailAccounts
            .FirstOrDefault(account => account.Email == fromEmail);

        if (emailAccount == null)
        {
            throw new InvalidOperationException($"The email account '{fromEmail}' was not found in the configuration.");
        }

        using var smtpClient = new SmtpClient(_emailSettings.SmtpServer);
        smtpClient.Port = _emailSettings.Port;
        smtpClient.EnableSsl = true;
        smtpClient.Credentials = new NetworkCredential(emailAccount.Email, emailAccount.Password);

        var mailMessage = new MailMessage
        {
            From = new MailAddress(emailAccount.Email),
            Subject = subject,
            Body = message,
            IsBodyHtml = true,
        };

        mailMessage.To.Add(toEmail);

        await smtpClient.SendMailAsync(mailMessage);
    }
}