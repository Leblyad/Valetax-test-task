using System.Reflection;
using System.Text.Json.Serialization;
using Events.API.Exceptions;
using Microsoft.OpenApi;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using SharedModels.Metrics;

namespace Events.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValetaxMetrics(configuration, serviceName: "events");
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
                Title = "Events API",
                Version = "v1",
                Description = "Profit/loss event ingest for Valetax commission system.",
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
