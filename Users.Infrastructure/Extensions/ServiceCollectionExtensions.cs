using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Options;
using Polly;
using Users.Application.Interfaces.Clients;
using Users.Infrastructure.Clients;
using Users.Infrastructure.Options;
using Users.Infrastructure.Outbox;

namespace Users.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    private const int HTTP_RETRY_ATTEMPTS = 3;

    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<WalletsHttpClientOptions>()
            .Bind(configuration.GetSection(WalletsHttpClientOptions.SECTION_NAME))
            .Validate(options => !string.IsNullOrWhiteSpace(options.BaseUrl), "Services:Wallets:BaseUrl is required.")
            .Validate(options => options.TimeoutSeconds > 0, "Services:Wallets:TimeoutSeconds must be greater than 0.")
            .ValidateOnStart();

        services.AddOptions<OutboxOptions>()
            .Bind(configuration.GetSection(OutboxOptions.SECTION_NAME))
            .Validate(options => options.PollIntervalSeconds > 0, "Outbox:PollIntervalSeconds must be greater than 0.")
            .Validate(options => options.BatchSize > 0, "Outbox:BatchSize must be greater than 0.")
            .Validate(options => options.MaxAttempts > 0, "Outbox:MaxAttempts must be greater than 0.")
            .ValidateOnStart();

        var timeoutSeconds = configuration.GetValue(
            $"{WalletsHttpClientOptions.SECTION_NAME}:TimeoutSeconds",
            30);

        services.AddHttpClient<IWalletApiClient, WalletApiClient>((serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<WalletsHttpClientOptions>>().Value;
                client.BaseAddress = new Uri(options.BaseUrl);
                client.Timeout = Timeout.InfiniteTimeSpan;
            })
            .AddResilienceHandler("wallets-http", builder =>
            {
                builder.AddRetry(new HttpRetryStrategyOptions
                {
                    MaxRetryAttempts = HTTP_RETRY_ATTEMPTS,
                    Delay = TimeSpan.FromSeconds(1),
                    BackoffType = DelayBackoffType.Exponential,
                    UseJitter = true,
                });
                builder.AddTimeout(TimeSpan.FromSeconds(timeoutSeconds));
            });

        services.AddHostedService<OutboxProcessor>();

        return services;
    }
}
