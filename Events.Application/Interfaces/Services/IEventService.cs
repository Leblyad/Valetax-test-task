using Events.Application.Dto;

namespace Events.Application.Interfaces.Services;

public interface IEventService
{
    Task<Guid> CreateEventAsync(CreateEventDto eventDto, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<EventDto>> GetEventsByUserAsync(Guid userExternalId, CancellationToken cancellationToken = default);

    Task<EventDto> GetEventByIdAsync(Guid eventExternalId, CancellationToken cancellationToken = default);
}
