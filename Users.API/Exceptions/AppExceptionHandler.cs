using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SharedModels.Exceptions;

namespace Users.API.Exceptions;

public sealed class AppExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<AppExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is AppException appException)
        {
            logger.LogWarning(
                exception,
                "Application error {ErrorCode} returned status {StatusCode} for {RequestMethod} {RequestPath}",
                appException.Code,
                appException.StatusCode,
                httpContext.Request.Method,
                httpContext.Request.Path.Value);

            return await WriteAsync(
                httpContext,
                exception,
                appException.StatusCode,
                appException.Code,
                appException.Message);
        }

        logger.LogError(
            exception,
            "Unhandled exception for {RequestMethod} {RequestPath}",
            httpContext.Request.Method,
            httpContext.Request.Path.Value);

        return await WriteAsync(
            httpContext,
            exception,
            500,
            "INTERNAL_SERVER_ERROR",
            "An unexpected error occurred.");
    }

    private async Task<bool> WriteAsync(
        HttpContext httpContext,
        Exception exception,
        int statusCode,
        string code,
        string detail)
    {
        httpContext.Response.StatusCode = statusCode;

        await problemDetailsService.WriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = code,
                Detail = detail,
                Extensions = { ["code"] = code },
            },
        });

        return true;
    }
}
