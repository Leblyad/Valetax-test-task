using System.Text.Json;
using Events.Application.Interfaces.Clients;
using Events.Application.Interfaces.Repositories;
using Events.Domain.Models;
using Events.Infrastructure.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharedModels.Metrics;
using SharedModels.Outbox;

namespace Events.Infrastructure.Outbox;

public sealed class OutboxProcessor(
    IServiceScopeFactory scopeFactory,
    IOptions<OutboxOptions> options,
    ILogger<OutboxProcessor> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessBatchAsync(stoppingToken);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                logger.LogError(ex, "Outbox processor batch failed");
            }

            await Task.Delay(TimeSpan.FromSeconds(options.Value.PollIntervalSeconds), stoppingToken);
        }
    }

    private async Task ProcessBatchAsync(CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var repositoryManager = scope.ServiceProvider.GetRequiredService<IRepositoryManager>();
        var walletsApiClient = scope.ServiceProvider.GetRequiredService<IWalletsApiClient>();
        var outboxOptions = options.Value;

        var messages = await repositoryManager.Outbox.GetPendingAsync(
            outboxOptions.BatchSize,
            outboxOptions.MaxAttempts,
            cancellationToken);

        foreach (var message in messages)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                await DispatchAsync(walletsApiClient, message, cancellationToken);
                message.ProcessedAtUtc = DateTime.UtcNow;
                message.LastError = null;
                ValetaxMeters.RecordOutboxProcessed("events", message.Type);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                message.AttemptCount++;
                message.LastError = Truncate(ex.Message);
                ValetaxMeters.RecordOutboxFailed("events", message.Type);

                logger.LogError(
                    ex,
                    "Failed to process outbox message {OutboxMessageId} of type {OutboxMessageType}. Attempt {AttemptCount}/{MaxAttempts}",
                    message.Id,
                    message.Type,
                    message.AttemptCount,
                    outboxOptions.MaxAttempts);
            }
        }

        if (messages.Count > 0)
        {
            await repositoryManager.SaveAsync(cancellationToken);
        }
    }

    private static async Task DispatchAsync(
        IWalletsApiClient walletsApiClient,
        OutboxMessage message,
        CancellationToken cancellationToken)
    {
        if (message.Type != OutboxMessageTypes.PROCESS_EVENT)
        {
            throw new InvalidOperationException($"Unknown outbox message type '{message.Type}'.");
        }

        var payload = JsonSerializer.Deserialize<ProcessEventPayload>(message.Payload)
            ?? throw new InvalidOperationException("Outbox payload is empty.");

        await walletsApiClient.ProcessEventAsync(
            payload.EventExternalId,
            payload.UserExternalId,
            payload.Profit,
            cancellationToken);
    }

    private static string Truncate(string value) =>
        value.Length <= 2000 ? value : value[..2000];

    private sealed record ProcessEventPayload(Guid EventExternalId, Guid UserExternalId, decimal Profit);
}
