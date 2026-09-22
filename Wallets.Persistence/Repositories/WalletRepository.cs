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
}
