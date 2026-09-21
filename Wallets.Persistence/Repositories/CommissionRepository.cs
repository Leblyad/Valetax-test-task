using Wallets.Application.Interfaces.Repositories;
using Wallets.Domain.Models;
using Wallets.Persistence.Context;

namespace Wallets.Persistence.Repositories;

public class CommissionRepository : RepositoryBase<Commission>, ICommissionRepository
{
    public CommissionRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }
}
