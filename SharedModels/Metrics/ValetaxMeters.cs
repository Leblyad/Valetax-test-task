using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace SharedModels.Metrics;

public static class ValetaxMeters
{
    public const string METER_NAME = "Valetax";

    public static readonly Meter Meter = new(METER_NAME);

    public static readonly Counter<long> HttpRequests =
        Meter.CreateCounter<long>("http.requests", description: "Total HTTP requests");

    public static readonly Histogram<double> HttpRequestDurationMs =
        Meter.CreateHistogram<double>("http.request.duration_ms", unit: "ms", description: "HTTP request duration");

    public static readonly Counter<long> OutboxProcessed =
        Meter.CreateCounter<long>("outbox.processed", description: "Successfully processed outbox messages");

    public static readonly Counter<long> OutboxFailed =
        Meter.CreateCounter<long>("outbox.failed", description: "Failed outbox message attempts");

    public static void RecordHttpRequest(string service, string method, int statusCode, double durationMs)
    {
        var tags = new TagList
        {
            { "service", service },
            { "method", method },
            { "status_code", statusCode },
        };

        HttpRequests.Add(1, tags);
        HttpRequestDurationMs.Record(durationMs, tags);
    }

    public static void RecordOutboxProcessed(string service, string messageType)
    {
        OutboxProcessed.Add(1, new TagList
        {
            { "service", service },
            { "type", messageType },
        });
    }

    public static void RecordOutboxFailed(string service, string messageType)
    {
        OutboxFailed.Add(1, new TagList
        {
            { "service", service },
            { "type", messageType },
        });
    }
}
