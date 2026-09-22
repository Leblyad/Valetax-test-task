namespace Events.Infrastructure.Options;

public sealed class OutboxOptions
{
    public const string SECTION_NAME = "Outbox";

    public int PollIntervalSeconds { get; set; } = 2;

    public int BatchSize { get; set; } = 20;

    public int MaxAttempts { get; set; } = 10;
}
