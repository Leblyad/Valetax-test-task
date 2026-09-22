using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SharedModels.Metrics;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddValetaxMetrics(
        this IServiceCollection services,
        IConfiguration configuration,
        string serviceName)
    {
        services.AddOptions<MetricsOptions>()
            .Bind(configuration.GetSection(MetricsOptions.SECTION_NAME))
            .PostConfigure(options =>
            {
                if (string.IsNullOrWhiteSpace(options.ServiceName))
                {
                    options.ServiceName = serviceName;
                }
            })
            .Validate(options => options.LogIntervalSeconds > 0, "Metrics:LogIntervalSeconds must be greater than 0.")
            .ValidateOnStart();

        services.AddHostedService<MetricsLoggingHostedService>();

        return services;
    }
}
