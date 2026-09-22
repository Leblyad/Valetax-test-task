using System.Net.Http.Json;
using SharedModels.Exceptions;
using Wallets.Application.Interfaces.Clients;

namespace Wallets.Infrastructure.Clients;

public sealed class UsersApiClient(HttpClient httpClient) : IUsersApiClient
{
    private const string SERVICE_NAME = "Users";

    public async Task<IReadOnlyList<PartnerRelationInfo>> GetPartnerRelationsAsync(
        Guid userExternalId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var relations = await httpClient.GetFromJsonAsync<List<PartnerRelationInfo>>(
                $"api/partner-relations/user/{userExternalId}",
                cancellationToken);

            return relations ?? [];
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or NotSupportedException or System.Text.Json.JsonException)
        {
            throw new ExternalServiceException(
                SERVICE_NAME,
                $"Failed to get partner relations for user '{userExternalId}'.",
                ex);
        }
    }
}
