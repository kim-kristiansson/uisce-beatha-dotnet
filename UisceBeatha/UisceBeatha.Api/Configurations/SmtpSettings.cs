namespace UisceBeatha.Api.Configurations;

public class SmtpSettings
{
    public string SmtpServer { get; init; } = string.Empty;
    public int Port { get; init; }
    public List<EmailAccountSettings> EmailAccounts { get; init; } = [];
}