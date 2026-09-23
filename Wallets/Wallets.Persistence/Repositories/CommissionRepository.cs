using Microsoft.EntityFrameworkCore;
using Wallets.Application.Interfaces.Repositories;
using Wallets.Domain.Models;
using Wallets.Persistence.Context;

namespace Wallets.Persistence.Repositories;

public class CommissionRepository : RepositoryBase<Commission>, ICommissionRepository
{
    public CommissionRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<IReadOnlyList<Commission>> GetByEventExternalIdAsync(
        Guid eventExternalId,
        bool trackChanges,
        CancellationToken cancellationToken = default) =>
        await FindByCondition(c => c.EventExternalId == eventExternalId, trackChanges)
            .OrderBy(c => c.Level)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Commission>> GetByUserExternalIdAsync(
        Guid userExternalId,
        bool trackChanges,
        CancellationToken cancellationToken = default) =>
        await FindByCondition(c => c.UserExternalId == userExternalId, trackChanges)
            .OrderByDescending(c => c.PaidAt)
            .ToListAsync(cancellationToken);

    public void CreateRange(IEnumerable<Commission> commissions) =>
        RepositoryContext.Set<Commission>().AddRange(commissions);
}
