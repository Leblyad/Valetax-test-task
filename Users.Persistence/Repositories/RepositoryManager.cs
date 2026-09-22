using Users.Application.Interfaces.Repositories;
using Users.Persistence.Context;

namespace Users.Persistence.Repositories;

public class RepositoryManager : IRepositoryManager
{
    private readonly RepositoryContext _repositoryContext;

    public RepositoryManager(
        RepositoryContext repositoryContext,
        IUserRepository userRepository,
        IPartnerRelationRepository partnerRelationRepository)
    {
        _repositoryContext = repositoryContext;
        User = userRepository;
        PartnerRelation = partnerRelationRepository;
    }

    public IUserRepository User { get; }

    public IPartnerRelationRepository PartnerRelation { get; }

    public Task SaveAsync(CancellationToken cancellationToken = default) =>
        _repositoryContext.SaveChangesAsync(cancellationToken);
}
