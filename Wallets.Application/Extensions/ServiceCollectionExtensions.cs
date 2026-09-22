using Microsoft.Extensions.DependencyInjection;
using Wallets.Application.Interfaces.Services;
using Wallets.Application.Services;

namespace Wallets.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IWalletService, WalletService>();
        services.AddScoped<ICommissionService, CommissionService>();

        return services;
    }
}
