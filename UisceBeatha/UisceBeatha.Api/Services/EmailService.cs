using System.Net;
using System.Net.Mail;
using UisceBeatha.Api.Configurations;
using UisceBeatha.Api.Services.Interfaces;

namespace UisceBeatha.Api.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;
    
    public EmailService(IConfiguration configuration)
    {
        _emailSettings = configuration.GetSection("EmailSettings").Get<EmailSettings>()
                         ?? throw new ArgumentNullException(nameof(configuration), "Email configuration section is missing or invalid.");

        // Validate email settings
        if (string.IsNullOrEmpty(_emailSettings.SmtpClient) || _emailSettings.Port == 0 || _emailSettings.EmailAccounts == null || _emailSettings.EmailAccounts.Count == 0)
        {
            throw new ArgumentException("Email configuration is invalid. Ensure all required fields are set in appsettings.json.");
        }
    }
    
    public async Task SendEmailAsync()
    {
        var smtpClient = new SmtpClient("smtp.forwardemail.net")
        {
            Port = 465, // Use 587 for STARTTLS or 465 for implicit SSL
            EnableSsl = true,
            Credentials = new NetworkCredential("info@gyllenedroppen.se", "b110559db3ce228857dbf13f")
        };
        
        
        var mailMessage = new MailMessage
        {
            From = new MailAddress("info@gyllenedroppen.se"),
            Subject = "Test Email",
            Body = "This is a test message.",
            IsBodyHtml = false
        
        mailMessage.To.Add("kim.kristiansson@hotmail        
        await smtpClient.SendMailAsync(mailMessage);
    }
}