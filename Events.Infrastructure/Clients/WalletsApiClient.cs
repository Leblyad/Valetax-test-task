using System.Net.Http.Json;
using Events.Application.Interfaces.Clients;
using SharedModels.Exceptions;

namespace Events.Infrastructure.Clients;

public sealed class WalletsApiClient(HttpClient httpClient) : IWalletsApiClient
{
    private const string SERVICE_NAME = "Wallets";

    public async Task ProcessEventAsync(
        Guid eventExternalId,
        Guid userExternalId,
        decimal profit,
        CancellationToken cancellationToken = default)
    {
        HttpResponseMessage response;
        try
        {
            response = await httpClient.PostAsJsonAsync(
                "api/commissions/process",
                new
                {
                    EventExternalId = eventExternalId,
                    UserExternalId = userExternalId,
                    Profit = profit,
                },
                cancellationToken);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            throw new ExternalServiceException(SERVICE_NAME, "Failed to process event commissions.", ex);
        }

        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        throw new ExternalServiceException(
            SERVICE_NAME,
            $"Failed to process event commissions. Status={(int)response.StatusCode}. Response={body}");
    }
}
