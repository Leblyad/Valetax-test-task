using Microsoft.EntityFrameworkCore;
using Events.Application.Interfaces.Repositories;
using Events.Domain.Models;
using Events.Persistence.Context;

namespace Events.Persistence.Repositories;

public class EventRepository : RepositoryBase<Event>, IEventRepository
{
    public EventRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public Task<Event?> GetByExternalIdAsync(
        Guid externalId,
        bool trackChanges,
        CancellationToken cancellationToken = default) =>
        FindByCondition(e => e.ExternalId == externalId, trackChanges)
            .SingleOrDefaultAsync(cancellationToken);

    public async Task<IReadOnlyList<Event>> GetByUserExternalIdAsync(
        Guid userExternalId,
        bool trackChanges,
        CancellationToken cancellationToken = default) =>
        await FindByCondition(e => e.UserExternalId == userExternalId, trackChanges)
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync(cancellationToken);
}
