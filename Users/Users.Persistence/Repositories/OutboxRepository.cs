using Microsoft.EntityFrameworkCore;
using Users.Application.Interfaces.Repositories;
using Users.Domain.Models;
using Users.Persistence.Context;

namespace Users.Persistence.Repositories;

public sealed class OutboxRepository(RepositoryContext repositoryContext)
    : RepositoryBase<OutboxMessage>(repositoryContext), IOutboxRepository
{
    public async Task<IReadOnlyList<OutboxMessage>> GetPendingAsync(
        int batchSize,
        int maxAttempts,
        CancellationToken cancellationToken = default)
    {
        return await FindByCondition(
                x => x.ProcessedAtUtc == null && x.AttemptCount < maxAttempts,
                trackChanges: true)
            .OrderBy(x => x.CreatedAtUtc)
            .Take(batchSize)
            .ToListAsync(cancellationToken);
    }
}
