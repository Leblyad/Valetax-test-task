using Users.Domain.Models;

namespace Users.Application.Interfaces.Repositories;

public interface IUserRepository : IRepositoryBase<User>
{
    Task<User?> GetByExternalIdAsync(Guid externalId, bool trackChanges, CancellationToken cancellationToken = default);
}
