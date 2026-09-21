using Users.Application.Interfaces.Repositories;
using Users.Domain.Models;
using Users.Persistence.Context;

namespace Users.Persistence.Repositories;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }
}
