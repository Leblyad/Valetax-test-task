namespace SharedModels.Metrics;

public sealed class MetricsOptions
{
    public const string SECTION_NAME = "Metrics";

    public string ServiceName { get; set; } = string.Empty;

    public int LogIntervalSeconds { get; set; } = 30;
}
