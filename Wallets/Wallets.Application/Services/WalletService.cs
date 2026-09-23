using MapsterMapper;
using SharedModels.Enums;
using Wallets.Application.Dto;
using Wallets.Application.Exceptions;
using Wallets.Application.Interfaces.Repositories;
using Wallets.Application.Interfaces.Services;
using Wallets.Domain.Models;

namespace Wallets.Application.Services;

public class WalletService(
    IRepositoryManager repositoryManager,
    IMapper mapper) : IWalletService
{
    public async Task<Guid> CreateWalletAsync(
        CreateWalletDto createWalletDto,
        CancellationToken cancellationToken = default)
    {
        var existing = await repositoryManager.Wallet.GetByUserExternalIdAsync(
            createWalletDto.UserExternalId,
            trackChanges: false,
            cancellationToken);

        if (existing is not null)
        {
            return existing.UserExternalId;
        }

        repositoryManager.Wallet.Create(mapper.Map<Wallet>(createWalletDto));
        await repositoryManager.SaveAsync(cancellationToken);

        return createWalletDto.UserExternalId;
    }

    public async Task<WalletDto> GetWalletAsync(
        Guid userExternalId,
        CancellationToken cancellationToken = default)
    {
        var wallet = await GetRequiredWalletAsync(userExternalId, trackChanges: false, cancellationToken);
        return mapper.Map<WalletDto>(wallet);
    }

    public async Task<IReadOnlyList<CommissionDto>> GetPayoutHistoryAsync(
        Guid userExternalId,
        CancellationToken cancellationToken = default)
    {
        var commissions = await repositoryManager.Commission.GetByUserExternalIdAsync(
            userExternalId,
            trackChanges: false,
            cancellationToken);

        return mapper.Map<List<CommissionDto>>(commissions.Where(c => c.PaidAt is not null));
    }

    public async Task SetSchemaAsync(
        Guid userExternalId,
        SchemaType schemaType,
        CancellationToken cancellationToken = default)
    {
        var wallet = await GetRequiredWalletAsync(userExternalId, trackChanges: true, cancellationToken);
        wallet.SchemaType = schemaType;
        await repositoryManager.SaveAsync(cancellationToken);
    }

    private async Task<Wallet> GetRequiredWalletAsync(
        Guid userExternalId,
        bool trackChanges,
        CancellationToken cancellationToken)
    {
        return await repositoryManager.Wallet.GetByUserExternalIdAsync(
            userExternalId,
            trackChanges,
            cancellationToken)
            ?? throw new WalletNotFoundException(userExternalId);
    }
}
