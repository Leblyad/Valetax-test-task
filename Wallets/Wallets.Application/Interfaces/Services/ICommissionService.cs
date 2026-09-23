using Wallets.Application.Dto;

namespace Wallets.Application.Interfaces.Services;

public interface ICommissionService
{
    Task ProcessEventAsync(ProcessEventDto processEventDto, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<CommissionDto>> GetCommissionsByEventAsync(Guid eventExternalId, CancellationToken cancellationToken = default);
}
