using Events.Domain.Models;

namespace Events.Application.Interfaces.Repositories;

public interface IOutboxRepository : IRepositoryBase<OutboxMessage>
{
    Task<IReadOnlyList<OutboxMessage>> GetPendingAsync(
        int batchSize,
        int maxAttempts,
        CancellationToken cancellationToken = default);
}
