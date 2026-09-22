using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharedModels.Enums;
using SharedModels.Metrics;
using SharedModels.Outbox;
using Users.Application.Interfaces.Clients;
using Users.Application.Interfaces.Repositories;
using Users.Domain.Models;
using Users.Infrastructure.Options;

namespace Users.Infrastructure.Outbox;

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
        var walletApiClient = scope.ServiceProvider.GetRequiredService<IWalletApiClient>();
        var outboxOptions = options.Value;

        var messages = await repositoryManager.Outbox.GetPendingAsync(
            outboxOptions.BatchSize,
            outboxOptions.MaxAttempts,
            cancellationToken);

        foreach (var message in messages)
        {
            try
            {
                await DispatchAsync(walletApiClient, message, cancellationToken);
                message.ProcessedAtUtc = DateTime.UtcNow;
                message.LastError = null;
                ValetaxMeters.RecordOutboxProcessed("users", message.Type);
            }
            catch (Exception ex)
            {
                message.AttemptCount++;
                message.LastError = Truncate(ex.Message);
                ValetaxMeters.RecordOutboxFailed("users", message.Type);

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
        IWalletApiClient walletApiClient,
        OutboxMessage message,
        CancellationToken cancellationToken)
    {
        if (message.Type != OutboxMessageTypes.CREATE_WALLET)
        {
            throw new InvalidOperationException($"Unknown outbox message type '{message.Type}'.");
        }

        var payload = JsonSerializer.Deserialize<CreateWalletPayload>(message.Payload)
            ?? throw new InvalidOperationException("Outbox payload is empty.");

        await walletApiClient.CreateWalletAsync(
            payload.ExternalId,
            payload.SchemaType,
            cancellationToken);
    }

    private static string Truncate(string value) =>
        value.Length <= 2000 ? value : value[..2000];

    private sealed record CreateWalletPayload(Guid ExternalId, SchemaType SchemaType);
}
