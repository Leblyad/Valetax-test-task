using SharedModels.Exceptions;

namespace Users.Application.Exceptions;

public sealed class CyclicPartnerRelationException : AppException
{
    public const string ERROR_CODE = "CYCLIC_PARTNER_RELATION";

    public Guid UserExternalId { get; }

    public Guid PartnerExternalId { get; }

    public CyclicPartnerRelationException(Guid userExternalId, Guid partnerExternalId)
        : base(
            ERROR_CODE,
            $"Cannot create a cyclic partner relation between '{userExternalId}' and '{partnerExternalId}'.",
            StatusCodes.Conflict)
    {
        UserExternalId = userExternalId;
        PartnerExternalId = partnerExternalId;
    }

    public CyclicPartnerRelationException(Guid userExternalId, Guid partnerExternalId, Exception innerException)
        : base(
            ERROR_CODE,
            $"Cannot create a cyclic partner relation between '{userExternalId}' and '{partnerExternalId}'.",
            StatusCodes.Conflict,
            innerException)
    {
        UserExternalId = userExternalId;
        PartnerExternalId = partnerExternalId;
    }
}
