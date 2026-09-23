using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wallets.Application.Interfaces.Repositories;
using Wallets.Persistence.Context;
using Wallets.Persistence.Repositories;

namespace Wallets.Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<RepositoryContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IWalletRepository, WalletRepository>();
        services.AddScoped<ICommissionRepository, CommissionRepository>();
        services.AddScoped<IRepositoryManager, RepositoryManager>();

        return services;
    }

    public static void ApplyMigrations(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<RepositoryContext>();
        context.Database.Migrate();
    }
}
