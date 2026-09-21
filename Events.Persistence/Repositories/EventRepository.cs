using Events.Application.Interfaces.Repositories;
using Events.Domain.Models;
using Events.Persistence.Context;

namespace Events.Persistence.Repositories;

public class EventRepository : RepositoryBase<Event>, IEventRepository
{
    public EventRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }
}
