using SharedModels.Exceptions;

namespace Wallets.Application.Exceptions;

public sealed class WalletNotFoundException : AppException
{
    public const string ERROR_CODE = "WALLET_NOT_FOUND";

    public Guid UserExternalId { get; }

    public WalletNotFoundException(Guid userExternalId)
        : base(ERROR_CODE, $"Wallet '{userExternalId}' was not found.", StatusCodes.NotFound)
    {
        UserExternalId = userExternalId;
    }

    public WalletNotFoundException(Guid userExternalId, Exception innerException)
        : base(ERROR_CODE, $"Wallet '{userExternalId}' was not found.", StatusCodes.NotFound, innerException)
    {
        UserExternalId = userExternalId;
    }
}
