using Users.Application.Interfaces.Repositories;
using Users.Domain.Models;
using Users.Persistence.Context;

namespace Users.Persistence.Repositories;

public class PartnerRepository : RepositoryBase<Partner>, IPartnerRepository
{
    public PartnerRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }
}
