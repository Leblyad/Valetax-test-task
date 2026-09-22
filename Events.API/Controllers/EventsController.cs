using Events.Application.Dto;
using Events.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Events.API.Controllers;

/// <summary>
/// Ingests profit/loss events and schedules commission processing via outbox.
/// </summary>
[ApiController]
[Route("api/events")]
[Produces("application/json")]
public class EventsController(IEventService eventService) : ControllerBase
{
    /// <summary>
    /// Creates an event (idempotent by ExternalId) and enqueues commission processing.
    /// </summary>
    /// <param name="eventDto">Event payload. Positive Profit accrues commissions.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Event external id.</returns>
    /// <response code="201">Event accepted.</response>
    /// <response code="400">Validation failed.</response>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Guid>> Create(
        [FromBody] CreateEventDto eventDto,
        CancellationToken cancellationToken)
    {
        var externalId = await eventService.CreateEventAsync(eventDto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { eventExternalId = externalId }, externalId);
    }

    /// <summary>
    /// Returns a single event by external id.
    /// </summary>
    /// <param name="eventExternalId">Event external id.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="200">Event found.</response>
    /// <response code="404">Event not found.</response>
    [HttpGet("{eventExternalId:guid}")]
    [ProducesResponseType(typeof(EventDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EventDto>> GetById(
        Guid eventExternalId,
        CancellationToken cancellationToken)
    {
        var eventDto = await eventService.GetEventByIdAsync(eventExternalId, cancellationToken);
        return Ok(eventDto);
    }

    /// <summary>
    /// Returns all events for a user.
    /// </summary>
    /// <param name="userExternalId">User external id.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpGet("user/{userExternalId:guid}")]
    [ProducesResponseType(typeof(IReadOnlyList<EventDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<EventDto>>> GetByUser(
        Guid userExternalId,
        CancellationToken cancellationToken)
    {
        var events = await eventService.GetEventsByUserAsync(userExternalId, cancellationToken);
        return Ok(events);
    }
}
