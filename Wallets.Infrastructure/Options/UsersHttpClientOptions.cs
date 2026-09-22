namespace Wallets.Infrastructure.Options;

public sealed class UsersHttpClientOptions
{
    public const string SECTION_NAME = "Services:Users";

    public string BaseUrl { get; set; } = string.Empty;

    public int TimeoutSeconds { get; set; } = 30;
}
