using SharedModels.Exceptions;

namespace Users.Application.Exceptions;

public sealed class UserNotFoundException : AppException
{
    public const string ERROR_CODE = "USER_NOT_FOUND";

    public Guid UserExternalId { get; }

    public UserNotFoundException(Guid userExternalId)
        : base(ERROR_CODE, $"User '{userExternalId}' was not found.", StatusCodes.NotFound)
    {
        UserExternalId = userExternalId;
    }

    public UserNotFoundException(Guid userExternalId, Exception innerException)
        : base(ERROR_CODE, $"User '{userExternalId}' was not found.", StatusCodes.NotFound, innerException)
    {
        UserExternalId = userExternalId;
    }
}
