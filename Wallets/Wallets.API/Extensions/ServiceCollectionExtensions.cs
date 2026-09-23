using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.OpenApi;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using SharedModels.Metrics;
using Wallets.API.Exceptions;

namespace Wallets.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValetaxMetrics(configuration, serviceName: "wallets");
        services.AddProblemDetails();
        services.AddExceptionHandler<AppExceptionHandler>();
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        services.AddFluentValidationAutoValidation();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Wallets API",
                Version = "v1",
                Description = "Wallets, commission schemas and payouts for Valetax.",
            });

            var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            }
        });
        services.AddHealthChecks();

        return services;
    }
}
