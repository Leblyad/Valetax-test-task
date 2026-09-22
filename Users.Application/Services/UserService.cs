using Users.Application.Dto;
using Users.Application.Interfaces.Services;

namespace Users.Application.Services;

public class UserService : IUserService
{
    public Task<Guid> CreateUserAsync(CreateUserDto createUserDto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> GetUserAsync(Guid externalId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
