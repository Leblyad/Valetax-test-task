namespace Events.Application.Interfaces.Repositories;

public interface IRepositoryManager
{
    IEventRepository Event { get; }

    Task SaveAsync(CancellationToken cancellationToken = default);
}
