using Microsoft.EntityFrameworkCore;
using Wallets.Application.Interfaces.Repositories;
using Wallets.Domain.Models;
using Wallets.Persistence.Context;

namespace Wallets.Persistence.Repositories;

public class WalletRepository : RepositoryBase<Wallet>, IWalletRepository
{
    public WalletRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public Task<Wallet?> GetByUserExternalIdAsync(
        Guid userExternalId,
        bool trackChanges,
        CancellationToken cancellationToken = default) =>
        FindByCondition(w => w.UserExternalId == userExternalId, trackChanges)
            .SingleOrDefaultAsync(cancellationToken);

    public Task IncrementBalanceAsync(
        Guid userExternalId,
        decimal amount,
        CancellationToken cancellationToken = default) =>
        RepositoryContext.Set<Wallet>()
            .Where(w => w.UserExternalId == userExternalId)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(w => w.Balance, w => w.Balance + amount),
                cancellationToken);
}
