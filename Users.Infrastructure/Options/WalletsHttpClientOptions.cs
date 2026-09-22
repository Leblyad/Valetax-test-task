namespace Users.Infrastructure.Options;

public sealed class WalletsHttpClientOptions
{
    public const string SECTION_NAME = "Services:Wallets";

    public string BaseUrl { get; set; } = string.Empty;

    public int TimeoutSeconds { get; set; } = 30;
}
