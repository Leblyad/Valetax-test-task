using Wallets.Application.Dto;
using SharedModels.Enums;

namespace Wallets.Application.Interfaces.Services;

public interface IWalletService
{
    Task<Guid> CreateWalletAsync(CreateWalletDto createWalletDto, CancellationToken cancellationToken = default);

    Task<WalletDto> GetWalletAsync(Guid userExternalId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<CommissionDto>> GetPayoutHistoryAsync(Guid userExternalId, CancellationToken cancellationToken = default);

    Task SetSchemaAsync(Guid userExternalId, SchemaType schemaType, CancellationToken cancellationToken = default);
}
