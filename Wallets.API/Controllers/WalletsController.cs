using Microsoft.AspNetCore.Mvc;
using SharedModels.Enums;
using Wallets.Application.Dto;
using Wallets.Application.Interfaces.Services;

namespace Wallets.API.Controllers;

/// <summary>
/// Manages wallets, commission schema and payout history.
/// </summary>
[ApiController]
[Route("api/wallets")]
[Produces("application/json")]
public class WalletsController(IWalletService walletService) : ControllerBase
{
    /// <summary>
    /// Creates a wallet for a user (idempotent by UserExternalId).
    /// </summary>
    /// <param name="createWalletDto">Wallet payload including SchemaType.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>User external id.</returns>
    /// <response code="201">Wallet created or already existed.</response>
    /// <response code="400">Validation failed.</response>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Guid>> Create(
        [FromBody] CreateWalletDto createWalletDto,
        CancellationToken cancellationToken)
    {
        var userExternalId = await walletService.CreateWalletAsync(createWalletDto, cancellationToken);
        return CreatedAtAction(nameof(GetByUserExternalId), new { userExternalId }, userExternalId);
    }

    /// <summary>
    /// Returns wallet balance and schema for a user.
    /// </summary>
    /// <param name="userExternalId">User external id.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="200">Wallet found.</response>
    /// <response code="404">Wallet not found.</response>
    [HttpGet("{userExternalId:guid}")]
    [ProducesResponseType(typeof(WalletDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WalletDto>> GetByUserExternalId(
        Guid userExternalId,
        CancellationToken cancellationToken)
    {
        var wallet = await walletService.GetWalletAsync(userExternalId, cancellationToken);
        return Ok(wallet);
    }

    /// <summary>
    /// Returns paid commission history for a user.
    /// </summary>
    /// <param name="userExternalId">User external id.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpGet("{userExternalId:guid}/payouts")]
    [ProducesResponseType(typeof(IReadOnlyList<CommissionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<CommissionDto>>> GetPayoutHistory(
        Guid userExternalId,
        CancellationToken cancellationToken)
    {
        var payouts = await walletService.GetPayoutHistoryAsync(userExternalId, cancellationToken);
        return Ok(payouts);
    }

    /// <summary>
    /// Changes commission schema for future calculations only.
    /// </summary>
    /// <param name="userExternalId">User external id.</param>
    /// <param name="schemaType">Linear or Fibonacci.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="204">Schema updated.</response>
    /// <response code="404">Wallet not found.</response>
    [HttpPut("{userExternalId:guid}/schema")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SetSchema(
        Guid userExternalId,
        [FromBody] SchemaType schemaType,
        CancellationToken cancellationToken)
    {
        await walletService.SetSchemaAsync(userExternalId, schemaType, cancellationToken);
        return NoContent();
    }
}
