using System.Net;
using System.Net.Mail;
using GylleneDroppen.Api.Configurations;
using GylleneDroppen.Api.Services.Interfaces;

namespace GylleneDroppen.Api.Services;

public class SmtpService : ISmtpService
{
    private readonly SmtpSettings _smtpSettings;
    
    public SmtpService(IConfiguration configuration)
    {
        _smtpSettings = configuration.GetSection("SmtpSettings").Get<SmtpSettings>()
                         ?? throw new ArgumentNullException(nameof(configuration), "Email configuration section is missing or invalid.");

        if (string.IsNullOrEmpty(_smtpSettings.SmtpServer) || _smtpSettings.Port == 0 || _smtpSettings.EmailAccounts == null || _smtpSettings.EmailAccounts.Count == 0)
        {
            throw new ArgumentException("Email configuration is invalid. Ensure all required fields are set in appsettings.json.");
        }
    }
    
    public async Task SendEmailAsync(string displayName, string fromEmail, string toEmail, string subject, string message)
    {
        var emailAccount = _smtpSettings.EmailAccounts
            .FirstOrDefault(account => account.Email == fromEmail);

        if (emailAccount == null)
        {
            throw new InvalidOperationException($"The email account '{fromEmail}' was not found in the configuration.");
        }

        using var smtpClient = new SmtpClient(_smtpSettings.SmtpServer);
        smtpClient.Port = _smtpSettings.Port;
        smtpClient.EnableSsl = true;
        smtpClient.Credentials = new NetworkCredential(emailAccount.Email, emailAccount.Password);
        smtpClient.UseDefaultCredentials = false;

        var mailMessage = new MailMessage
        {
            From = new MailAddress(emailAccount.Email, displayName),
            Subject = subject,
            Body = message,
            IsBodyHtml = true,
        };

        mailMessage.To.Add(toEmail);

        await smtpClient.SendMailAsync(mailMessage);
    }
}