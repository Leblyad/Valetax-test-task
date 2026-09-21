using Events.Application.Interfaces.Repositories;
using Events.Persistence.Context;

namespace Events.Persistence.Repositories;

public class RepositoryManager : IRepositoryManager
{
    private readonly RepositoryContext _repositoryContext;

    public RepositoryManager(
        RepositoryContext repositoryContext,
        IEventRepository eventRepository)
    {
        _repositoryContext = repositoryContext;
        Event = eventRepository;
    }

    public IEventRepository Event { get; }

    public Task SaveAsync(CancellationToken cancellationToken = default) =>
        _repositoryContext.SaveChangesAsync(cancellationToken);
}
