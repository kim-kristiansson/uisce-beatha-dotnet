namespace UisceBeatha.Api.Configurations;

public class EmailSettings
{
    public string? SmtpClient { get; init; }
    public int Port { get; init; }
    public List<EmailAccountSettings> EmailAccounts { get; init; } = [];
}