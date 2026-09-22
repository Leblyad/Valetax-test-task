using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace SharedModels.Metrics;

public sealed class RequestMetricsMiddleware(
    RequestDelegate next,
    IOptions<MetricsOptions> options)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            await next(context);
        }
        finally
        {
            stopwatch.Stop();
            ValetaxMeters.RecordHttpRequest(
                options.Value.ServiceName,
                context.Request.Method,
                context.Response.StatusCode,
                stopwatch.Elapsed.TotalMilliseconds);
        }
    }
}
