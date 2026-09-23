using Events.Persistence.Extensions;
using SharedModels.Metrics;

namespace Events.API.Extensions;

public static class ApplicationBuilderExtensions
{
    public static WebApplication UseApiPipeline(this WebApplication app)
    {
        app.Services.ApplyMigrations();
        app.UseExceptionHandler();
        app.UseMiddleware<RequestMetricsMiddleware>();

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Events API v1");
        });

        app.UseHttpsRedirection();
        app.MapHealthChecks("/health");
        app.MapControllers();

        return app;
    }
}
