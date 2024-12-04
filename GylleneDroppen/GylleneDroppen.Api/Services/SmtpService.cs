using System.Net;
using System.Net.Mail;
using GylleneDroppen.Api.Configurations;
using GylleneDroppen.Api.Services.Interfaces;

namespace GylleneDroppen.Api.Services;

public class SmtpService : ISmtpService
{
    private readonly SmtpConfig _smtpConfig;
    
    public SmtpService(IConfiguration configuration)
    {
        _smtpConfig = configuration.GetSection("SmtpSettings").Get<SmtpConfig>()
                         ?? throw new ArgumentNullException(nameof(configuration), "Email configuration section is missing or invalid.");

        if (string.IsNullOrEmpty(_smtpConfig.SmtpServer) || _smtpConfig.Port == 0 || _smtpConfig.EmailAccounts == null || _smtpConfig.EmailAccounts.Count == 0)
        {
            throw new ArgumentException("Email configuration is invalid. Ensure all required fields are set in appsettings.json.");
        }
    }
    
    public async Task SendEmailAsync(string displayName, string fromEmail, string toEmail, string subject, string message)
    {
        var emailAccount = _smtpConfig.EmailAccounts
            .FirstOrDefault(account => account.Email == fromEmail);

        if (emailAccount == null)
        {
            throw new InvalidOperationException($"The email account '{fromEmail}' was not found in the configuration.");
        }

        using var smtpClient = new SmtpClient(_smtpConfig.SmtpServer);
        smtpClient.Port = _smtpConfig.Port;
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