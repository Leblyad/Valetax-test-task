using Wallets.Application.Dto;
using Wallets.Application.Interfaces.Services;
using SharedModels.Enums;

namespace Wallets.Application.Services;

public class WalletService : IWalletService
{
    public Task<Guid> CreateWalletAsync(CreateWalletDto createWalletDto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<WalletDto> GetWalletAsync(Guid userExternalId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<CommissionDto>> GetPayoutHistoryAsync(Guid userExternalId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task SetSchemaAsync(Guid userExternalId, SchemaType schemaType, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
