using Events.Application.Interfaces.Repositories;
using Events.Persistence.Context;

namespace Events.Persistence.Repositories;

public class RepositoryManager : IRepositoryManager
{
    private readonly RepositoryContext _repositoryContext;

    public RepositoryManager(
        RepositoryContext repositoryContext,
        IEventRepository eventRepository,
        IOutboxRepository outboxRepository)
    {
        _repositoryContext = repositoryContext;
        Event = eventRepository;
        Outbox = outboxRepository;
    }

    public IEventRepository Event { get; }

    public IOutboxRepository Outbox { get; }

    public Task SaveAsync(CancellationToken cancellationToken = default) =>
        _repositoryContext.SaveChangesAsync(cancellationToken);
}
