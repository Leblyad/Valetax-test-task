using SharedModels.Exceptions;

namespace Users.Application.Exceptions;

public sealed class PartnerRelationAlreadyExistsException : AppException
{
    public const string ERROR_CODE = "PARTNER_RELATION_ALREADY_EXISTS";

    public Guid UserExternalId { get; }

    public PartnerRelationAlreadyExistsException(Guid userExternalId)
        : base(ERROR_CODE, $"User '{userExternalId}' already has a partner relation.", StatusCodes.Conflict)
    {
        UserExternalId = userExternalId;
    }

    public PartnerRelationAlreadyExistsException(Guid userExternalId, Exception innerException)
        : base(ERROR_CODE, $"User '{userExternalId}' already has a partner relation.", StatusCodes.Conflict, innerException)
    {
        UserExternalId = userExternalId;
    }
}
