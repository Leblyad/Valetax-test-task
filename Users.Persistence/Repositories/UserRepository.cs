using Microsoft.EntityFrameworkCore;
using Users.Application.Interfaces.Repositories;
using Users.Domain.Models;
using Users.Persistence.Context;

namespace Users.Persistence.Repositories;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public Task<User?> GetByExternalIdAsync(
        Guid externalId,
        bool trackChanges,
        CancellationToken cancellationToken = default) =>
        FindByCondition(u => u.ExternalId == externalId, trackChanges)
            .SingleOrDefaultAsync(cancellationToken);
}
