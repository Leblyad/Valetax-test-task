using Events.Application.Dto;
using Events.Application.Interfaces.Services;

namespace Events.Application.Services;

public class EventService : IEventService
{
    public Task<Guid> CreateEventAsync(CreateEventDto eventDto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<EventDto>> GetEventsByUserAsync(Guid userExternalId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<EventDto> GetEventByIdAsync(Guid eventExternalId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
