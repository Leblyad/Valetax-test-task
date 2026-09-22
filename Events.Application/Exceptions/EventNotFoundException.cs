using SharedModels.Exceptions;

namespace Events.Application.Exceptions;

public sealed class EventNotFoundException : AppException
{
    public const string ERROR_CODE = "EVENT_NOT_FOUND";

    public Guid EventExternalId { get; }

    public EventNotFoundException(Guid eventExternalId)
        : base(ERROR_CODE, $"Event '{eventExternalId}' was not found.", StatusCodes.NotFound)
    {
        EventExternalId = eventExternalId;
    }

    public EventNotFoundException(Guid eventExternalId, Exception innerException)
        : base(ERROR_CODE, $"Event '{eventExternalId}' was not found.", StatusCodes.NotFound, innerException)
    {
        EventExternalId = eventExternalId;
    }
}
