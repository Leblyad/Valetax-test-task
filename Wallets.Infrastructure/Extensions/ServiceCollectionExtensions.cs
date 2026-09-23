using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Options;
using Polly;
using Wallets.Application.Interfaces.Clients;
using Wallets.Infrastructure.Clients;
using Wallets.Infrastructure.Options;

namespace Wallets.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    private const int HTTP_RETRY_ATTEMPTS = 3;
    private const double CIRCUIT_BREAKER_FAILURE_RATIO = 0.5;
    private const int CIRCUIT_BREAKER_MINIMUM_THROUGHPUT = 5;
    private const int CIRCUIT_BREAKER_SAMPLING_SECONDS = 30;
    private const int CIRCUIT_BREAKER_BREAK_SECONDS = 30;

    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<UsersHttpClientOptions>()
            .Bind(configuration.GetSection(UsersHttpClientOptions.SECTION_NAME))
            .Validate(options => !string.IsNullOrWhiteSpace(options.BaseUrl), "Services:Users:BaseUrl is required.")
            .Validate(options => options.TimeoutSeconds > 0, "Services:Users:TimeoutSeconds must be greater than 0.")
            .ValidateOnStart();

        var timeoutSeconds = configuration.GetValue(
            $"{UsersHttpClientOptions.SECTION_NAME}:TimeoutSeconds",
            30);

        services.AddHttpClient<IUsersApiClient, UsersApiClient>((serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<UsersHttpClientOptions>>().Value;
                client.BaseAddress = new Uri(options.BaseUrl);
                client.Timeout = Timeout.InfiniteTimeSpan;
            })
            .AddResilienceHandler("users-http", builder =>
            {
                builder.AddRetry(new HttpRetryStrategyOptions
                {
                    MaxRetryAttempts = HTTP_RETRY_ATTEMPTS,
                    Delay = TimeSpan.FromSeconds(1),
                    BackoffType = DelayBackoffType.Exponential,
                    UseJitter = true,
                });
                builder.AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions
                {
                    SamplingDuration = TimeSpan.FromSeconds(CIRCUIT_BREAKER_SAMPLING_SECONDS),
                    FailureRatio = CIRCUIT_BREAKER_FAILURE_RATIO,
                    MinimumThroughput = CIRCUIT_BREAKER_MINIMUM_THROUGHPUT,
                    BreakDuration = TimeSpan.FromSeconds(CIRCUIT_BREAKER_BREAK_SECONDS),
                });
                builder.AddTimeout(TimeSpan.FromSeconds(timeoutSeconds));
            });

        return services;
    }
}
