using Users.Application.Dto;

namespace Users.Application.Interfaces.Services;

public interface IUserService
{
    Task<Guid> CreateUserAsync(CreateUserDto createUserDto, CancellationToken cancellationToken = default);

    Task<UserDto> GetUserAsync(Guid externalId, CancellationToken cancellationToken = default);
}
