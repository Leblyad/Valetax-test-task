using Microsoft.EntityFrameworkCore;
using Wallets.Domain.Models;

namespace Wallets.Persistence.Context;

public class RepositoryContext(DbContextOptions<RepositoryContext> options) : DbContext(options)
{
    public DbSet<Wallet> Wallets => Set<Wallet>();

    public DbSet<Commission> Commissions => Set<Commission>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RepositoryContext).Assembly);
    }
}
