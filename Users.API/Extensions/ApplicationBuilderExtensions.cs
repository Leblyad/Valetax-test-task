using SharedModels.Metrics;
using Users.Persistence.Extensions;

namespace Users.API.Extensions;

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
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Users API v1");
        });

        app.UseHttpsRedirection();
        app.MapHealthChecks("/health");
        app.MapControllers();

        return app;
    }
}
