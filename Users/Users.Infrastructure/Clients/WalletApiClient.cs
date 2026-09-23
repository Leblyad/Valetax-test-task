using System.Net.Http.Json;
using SharedModels.Enums;
using SharedModels.Exceptions;
using Users.Application.Interfaces.Clients;

namespace Users.Infrastructure.Clients;

public sealed class WalletApiClient(HttpClient httpClient) : IWalletApiClient
{
    private const string SERVICE_NAME = "Wallets";

    public async Task CreateWalletAsync(
        Guid userExternalId,
        SchemaType schemaType,
        CancellationToken cancellationToken = default)
    {
        HttpResponseMessage response;
        try
        {
            response = await httpClient.PostAsJsonAsync(
                "api/wallets",
                new
                {
                    UserExternalId = userExternalId,
                    Balance = 0m,
                    SchemaType = schemaType,
                },
                cancellationToken);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            throw new ExternalServiceException(SERVICE_NAME, "Failed to create wallet.", ex);
        }

        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        throw new ExternalServiceException(
            SERVICE_NAME,
            $"Failed to create wallet. Status={(int)response.StatusCode}. Response={body}");
    }
}
