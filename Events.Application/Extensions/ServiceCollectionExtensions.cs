using Events.Application.Interfaces.Services;
using Events.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Events.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IEventService, EventService>();

        return services;
    }
}
