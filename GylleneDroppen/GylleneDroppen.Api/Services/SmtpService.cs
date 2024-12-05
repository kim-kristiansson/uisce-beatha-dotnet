using System.Net;
using System.Net.Mail;
using GylleneDroppen.Api.Configurations;
using GylleneDroppen.Api.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace GylleneDroppen.Api.Services;

public class SmtpService(IOptions<SmtpConfig> smptConfigOptions, IOptions<EmailAccountsConfig> emailAccountsOptions) : ISmtpService
{
    private readonly SmtpConfig _smtpConfig = smptConfigOptions.Value;
    private readonly EmailAccountsConfig _emailAccountsConfig = emailAccountsOptions.Value;

    public async Task SendEmailAsync(string displayName, string fromEmail, string toEmail, string subject, string message)
    {
        var emailAccount = _emailAccountsConfig[$"{fromEmail}"];

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