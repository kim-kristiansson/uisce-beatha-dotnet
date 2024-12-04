namespace GylleneDroppen.Api.Configurations;

public class SmtpConfig
{
    public string SmtpServer { get; init; } = string.Empty;
    public int Port { get; init; }
    public List<EmailAccountConfig> EmailAccounts { get; init; } = [];
}