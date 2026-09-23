using Wallets.Domain.Models;

namespace Wallets.Application.Interfaces.Repositories;

public interface ICommissionRepository : IRepositoryBase<Commission>
{
    Task<IReadOnlyList<Commission>> GetByEventExternalIdAsync(
        Guid eventExternalId,
        bool trackChanges,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Commission>> GetByUserExternalIdAsync(
        Guid userExternalId,
        bool trackChanges,
        CancellationToken cancellationToken = default);

    void CreateRange(IEnumerable<Commission> commissions);
}
