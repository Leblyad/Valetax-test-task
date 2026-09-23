using Wallets.Application.Interfaces.Repositories;
using Wallets.Persistence.Context;

namespace Wallets.Persistence.Repositories;

public class RepositoryManager : IRepositoryManager
{
    private readonly RepositoryContext _repositoryContext;

    public RepositoryManager(
        RepositoryContext repositoryContext,
        IWalletRepository walletRepository,
        ICommissionRepository commissionRepository)
    {
        _repositoryContext = repositoryContext;
        Wallet = walletRepository;
        Commission = commissionRepository;
    }

    public IWalletRepository Wallet { get; }

    public ICommissionRepository Commission { get; }

    public Task SaveAsync(CancellationToken cancellationToken = default) =>
        _repositoryContext.SaveChangesAsync(cancellationToken);
}
