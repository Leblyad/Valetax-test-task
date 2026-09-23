using System.Text.Json;
using Events.Application.Dto;
using Events.Application.Exceptions;
using Events.Application.Interfaces.Repositories;
using Events.Application.Interfaces.Services;
using Events.Domain.Models;
using MapsterMapper;
using SharedModels.Outbox;

namespace Events.Application.Services;

public class EventService(
    IRepositoryManager repositoryManager,
    IMapper mapper) : IEventService
{
    public async Task<Guid> CreateEventAsync(CreateEventDto eventDto, CancellationToken cancellationToken = default)
    {
        var existing = await repositoryManager.Event.GetByExternalIdAsync(
            eventDto.ExternalId,
            trackChanges: false,
            cancellationToken);

        if (existing is not null)
        {
            return existing.ExternalId;
        }

        repositoryManager.Event.Create(mapper.Map<Event>(eventDto));
        repositoryManager.Outbox.Create(CreateProcessEventOutboxMessage(eventDto));
        await repositoryManager.SaveAsync(cancellationToken);

        return eventDto.ExternalId;
    }

    public async Task<IReadOnlyList<EventDto>> GetEventsByUserAsync(
        Guid userExternalId,
        CancellationToken cancellationToken = default)
    {
        var events = await repositoryManager.Event.GetByUserExternalIdAsync(
            userExternalId,
            trackChanges: false,
            cancellationToken);

        return mapper.Map<List<EventDto>>(events);
    }

    public async Task<EventDto> GetEventByIdAsync(
        Guid eventExternalId,
        CancellationToken cancellationToken = default)
    {
        var entity = await repositoryManager.Event.GetByExternalIdAsync(
            eventExternalId,
            trackChanges: false,
            cancellationToken)
            ?? throw new EventNotFoundException(eventExternalId);

        return mapper.Map<EventDto>(entity);
    }

    private static OutboxMessage CreateProcessEventOutboxMessage(CreateEventDto eventDto) =>
        new()
        {
            Id = Guid.NewGuid(),
            Type = OutboxMessageTypes.PROCESS_EVENT,
            Payload = JsonSerializer.Serialize(new
            {
                EventExternalId = eventDto.ExternalId,
                eventDto.UserExternalId,
                eventDto.Profit,
            }),
            CreatedAtUtc = DateTime.UtcNow,
        };
}
