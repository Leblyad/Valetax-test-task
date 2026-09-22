using Users.Domain.Models;

namespace Users.Application.Interfaces.Repositories;

public interface IOutboxRepository : IRepositoryBase<OutboxMessage>
{
    Task<IReadOnlyList<OutboxMessage>> GetPendingAsync(
        int batchSize,
        int maxAttempts,
        CancellationToken cancellationToken = default);
}
