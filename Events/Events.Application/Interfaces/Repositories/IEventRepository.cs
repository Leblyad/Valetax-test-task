using Events.Domain.Models;

namespace Events.Application.Interfaces.Repositories;

public interface IEventRepository : IRepositoryBase<Event>
{
    Task<Event?> GetByExternalIdAsync(Guid externalId, bool trackChanges, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Event>> GetByUserExternalIdAsync(
        Guid userExternalId,
        bool trackChanges,
        CancellationToken cancellationToken = default);
}
