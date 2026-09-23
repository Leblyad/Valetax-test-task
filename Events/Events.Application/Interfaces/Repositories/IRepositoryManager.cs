namespace Events.Application.Interfaces.Repositories;

public interface IRepositoryManager
{
    IEventRepository Event { get; }

    IOutboxRepository Outbox { get; }

    Task SaveAsync(CancellationToken cancellationToken = default);
}
