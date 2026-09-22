namespace SharedModels.Exceptions;

public sealed class ExternalServiceException : AppException
{
    public const string ERROR_CODE = "EXTERNAL_SERVICE_ERROR";

    public string ServiceName { get; }

    public ExternalServiceException(string serviceName, string message)
        : base(ERROR_CODE, message, StatusCodes.BadGateway)
    {
        ServiceName = serviceName;
    }

    public ExternalServiceException(string serviceName, string message, Exception innerException)
        : base(ERROR_CODE, message, StatusCodes.BadGateway, innerException)
    {
        ServiceName = serviceName;
    }
}
