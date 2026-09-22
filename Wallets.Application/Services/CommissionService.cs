using Wallets.Application.Dto;
using Wallets.Application.Interfaces.Services;

namespace Wallets.Application.Services;

public class CommissionService : ICommissionService
{
    public Task ProcessEventAsync(ProcessEventDto processEventDto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<CommissionDto>> GetCommissionsByEventAsync(Guid eventExternalId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
