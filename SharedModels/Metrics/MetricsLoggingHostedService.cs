using System.Collections.Concurrent;
using System.Diagnostics.Metrics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SharedModels.Metrics;

public sealed class MetricsLoggingHostedService : BackgroundService
{
    public const string LOGGER_NAME = "Metrics";

    private readonly ILogger _logger;
    private readonly MetricsOptions _options;
    private readonly MeterListener _listener = new();
    private readonly ConcurrentDictionary<string, long> _counterTotals = new();
    private readonly ConcurrentDictionary<string, HistogramAggregate> _histograms = new();

    public MetricsLoggingHostedService(
        ILoggerFactory loggerFactory,
        IOptions<MetricsOptions> options)
    {
        _logger = loggerFactory.CreateLogger(LOGGER_NAME);
        _options = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _listener.InstrumentPublished = (instrument, listener) =>
        {
            if (instrument.Meter.Name == ValetaxMeters.METER_NAME)
            {
                listener.EnableMeasurementEvents(instrument);
            }
        };

        _listener.SetMeasurementEventCallback<long>(OnLongMeasurement);
        _listener.SetMeasurementEventCallback<double>(OnDoubleMeasurement);
        _listener.Start();

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(_options.LogIntervalSeconds), stoppingToken);
            WriteSnapshot();
        }
    }

    public override void Dispose()
    {
        _listener.Dispose();
        base.Dispose();
    }

    private void OnLongMeasurement(
        Instrument instrument,
        long measurement,
        ReadOnlySpan<KeyValuePair<string, object?>> tags,
        object? state)
    {
        var key = BuildKey(instrument.Name, tags);
        _counterTotals.AddOrUpdate(key, measurement, (_, current) => current + measurement);
    }

    private void OnDoubleMeasurement(
        Instrument instrument,
        double measurement,
        ReadOnlySpan<KeyValuePair<string, object?>> tags,
        object? state)
    {
        var key = BuildKey(instrument.Name, tags);
        _histograms.AddOrUpdate(
            key,
            _ => new HistogramAggregate(measurement, 1),
            (_, current) => new HistogramAggregate(current.Sum + measurement, current.Count + 1));
    }

    private void WriteSnapshot()
    {
        foreach (var (key, value) in _counterTotals)
        {
            _logger.LogInformation(
                "Metric {MetricName} value {MetricValue} service {ServiceName}",
                key,
                value,
                _options.ServiceName);
        }

        foreach (var (key, aggregate) in _histograms)
        {
            _logger.LogInformation(
                "Metric {MetricName} count {MetricCount} sum {MetricSum} avg {MetricAvg} service {ServiceName}",
                key,
                aggregate.Count,
                Math.Round(aggregate.Sum, 2),
                Math.Round(aggregate.Average, 2),
                _options.ServiceName);
        }
    }

    private static string BuildKey(string instrumentName, ReadOnlySpan<KeyValuePair<string, object?>> tags)
    {
        if (tags.Length == 0)
        {
            return instrumentName;
        }

        var parts = new string[tags.Length];
        for (var i = 0; i < tags.Length; i++)
        {
            parts[i] = $"{tags[i].Key}={tags[i].Value}";
        }

        Array.Sort(parts, StringComparer.Ordinal);
        return $"{instrumentName}[{string.Join(',', parts)}]";
    }

    private sealed class HistogramAggregate(double sum, long count)
    {
        public double Sum { get; } = sum;

        public long Count { get; } = count;

        public double Average => Count == 0 ? 0 : Sum / Count;
    }
}
