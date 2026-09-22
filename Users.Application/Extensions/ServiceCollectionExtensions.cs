using Microsoft.Extensions.DependencyInjection;
using Users.Application.Interfaces.Services;
using Users.Application.Services;

namespace Users.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPartnerRelationService, PartnerRelationService>();

        return services;
    }
}
