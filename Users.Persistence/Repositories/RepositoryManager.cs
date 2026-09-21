using Users.Application.Interfaces.Repositories;
using Users.Persistence.Context;

namespace Users.Persistence.Repositories;

public class RepositoryManager : IRepositoryManager
{
    private readonly RepositoryContext _repositoryContext;

    public RepositoryManager(
        RepositoryContext repositoryContext,
        IUserRepository userRepository,
        IPartnerRepository partnerRepository)
    {
        _repositoryContext = repositoryContext;
        User = userRepository;
        Partner = partnerRepository;
    }

    public IUserRepository User { get; }

    public IPartnerRepository Partner { get; }

    public Task SaveAsync(CancellationToken cancellationToken = default) =>
        _repositoryContext.SaveChangesAsync(cancellationToken);
}
