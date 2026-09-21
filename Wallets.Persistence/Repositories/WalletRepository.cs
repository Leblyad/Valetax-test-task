using Wallets.Application.Interfaces.Repositories;
using Wallets.Domain.Models;
using Wallets.Persistence.Context;

namespace Wallets.Persistence.Repositories;

public class WalletRepository : RepositoryBase<Wallet>, IWalletRepository
{
    public WalletRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }
}
