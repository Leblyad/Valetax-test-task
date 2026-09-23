namespace Events.Application.Interfaces.Clients;

public interface IWalletsApiClient
{
    Task ProcessEventAsync(
        Guid eventExternalId,
        Guid userExternalId,
        decimal profit,
        CancellationToken cancellationToken = default);
}
