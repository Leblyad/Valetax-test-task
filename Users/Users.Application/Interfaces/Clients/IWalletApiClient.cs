using Users.Application.Dto;
using SharedModels.Enums;

namespace Users.Application.Interfaces.Clients;

public interface IWalletApiClient
{
    Task CreateWalletAsync(Guid userExternalId, SchemaType schemaType, CancellationToken cancellationToken = default);
}
