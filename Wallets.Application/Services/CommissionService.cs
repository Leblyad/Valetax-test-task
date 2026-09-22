using MapsterMapper;
using Wallets.Application.Dto;
using Wallets.Application.Exceptions;
using Wallets.Application.Helpers;
using Wallets.Application.Interfaces.Clients;
using Wallets.Application.Interfaces.Repositories;
using Wallets.Application.Interfaces.Services;
using Wallets.Domain.Models;

namespace Wallets.Application.Services;

public class CommissionService(
    IRepositoryManager repositoryManager,
    IUsersApiClient usersApiClient,
    IMapper mapper) : ICommissionService
{
    public const int MAX_LEVEL = 10;

    public async Task ProcessEventAsync(
        ProcessEventDto processEventDto,
        CancellationToken cancellationToken = default)
    {
        if (processEventDto.Profit <= 0)
        {
            return;
        }

        var existing = await repositoryManager.Commission.GetByEventExternalIdAsync(
            processEventDto.EventExternalId,
            trackChanges: false,
            cancellationToken);

        if (existing.Count > 0)
        {
            return;
        }

        var relations = await usersApiClient.GetPartnerRelationsAsync(
            processEventDto.UserExternalId,
            cancellationToken);

        var commissions = await BuildCommissionsAsync(processEventDto, relations, cancellationToken);
        if (commissions.Count == 0)
        {
            return;
        }

        repositoryManager.Commission.CreateRange(commissions);
        await repositoryManager.SaveAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<CommissionDto>> GetCommissionsByEventAsync(
        Guid eventExternalId,
        CancellationToken cancellationToken = default)
    {
        var commissions = await repositoryManager.Commission.GetByEventExternalIdAsync(
            eventExternalId,
            trackChanges: false,
            cancellationToken);

        return mapper.Map<List<CommissionDto>>(commissions);
    }

    private async Task<List<Commission>> BuildCommissionsAsync(
        ProcessEventDto processEventDto,
        IReadOnlyList<PartnerRelationInfo> relations,
        CancellationToken cancellationToken)
    {
        var paidAt = DateTime.UtcNow;
        var commissions = new List<Commission>();

        foreach (var relation in relations.Where(r => r.Level is >= 1 and <= MAX_LEVEL).OrderBy(r => r.Level))
        {
            var wallet = await repositoryManager.Wallet.GetByUserExternalIdAsync(
                relation.PartnerExternalId,
                trackChanges: true,
                cancellationToken)
                ?? throw new WalletNotFoundException(relation.PartnerExternalId);

            var amount = CommissionCalculator.Calculate(
                wallet.SchemaType,
                relation.Level,
                processEventDto.Profit);

            if (amount <= 0)
            {
                continue;
            }

            wallet.Balance += amount;

            commissions.Add(new Commission
            {
                EventExternalId = processEventDto.EventExternalId,
                UserExternalId = relation.PartnerExternalId,
                SchemaType = wallet.SchemaType,
                Amount = amount,
                Level = relation.Level,
                PaidAt = paidAt,
            });
        }

        return commissions;
    }
}
