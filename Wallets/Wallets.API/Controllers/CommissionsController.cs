using Microsoft.AspNetCore.Mvc;
using Wallets.Application.Dto;
using Wallets.Application.Interfaces.Services;

namespace Wallets.API.Controllers;

/// <summary>
/// Processes events into partner commissions and exposes commission queries.
/// </summary>
[ApiController]
[Route("api/commissions")]
[Produces("application/json")]
public class CommissionsController(ICommissionService commissionService) : ControllerBase
{
    /// <summary>
    /// Accrues commissions for a positive-profit event (idempotent by EventExternalId).
    /// </summary>
    /// <param name="processEventDto">Event id, source user and profit.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="202">Processing accepted.</response>
    /// <response code="400">Validation failed.</response>
    /// <response code="404">Partner wallet missing.</response>
    /// <response code="502">Users service unavailable.</response>
    [HttpPost("process")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status502BadGateway)]
    public async Task<IActionResult> ProcessEvent(
        [FromBody] ProcessEventDto processEventDto,
        CancellationToken cancellationToken)
    {
        await commissionService.ProcessEventAsync(processEventDto, cancellationToken);
        return Accepted();
    }

    /// <summary>
    /// Returns commissions generated for an event.
    /// </summary>
    /// <param name="eventExternalId">Event external id.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpGet("event/{eventExternalId:guid}")]
    [ProducesResponseType(typeof(IReadOnlyList<CommissionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<CommissionDto>>> GetByEvent(
        Guid eventExternalId,
        CancellationToken cancellationToken)
    {
        var commissions = await commissionService.GetCommissionsByEventAsync(eventExternalId, cancellationToken);
        return Ok(commissions);
    }
}
