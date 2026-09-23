namespace Wallets.Application.Interfaces.Repositories;

public interface IRepositoryManager
{
    IWalletRepository Wallet { get; }

    ICommissionRepository Commission { get; }

    Task SaveAsync(CancellationToken cancellationToken = default);

    Task ExecuteInTransactionAsync(
        Func<CancellationToken, Task> action,
        CancellationToken cancellationToken = default);
}
