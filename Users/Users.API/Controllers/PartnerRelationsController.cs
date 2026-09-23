using Microsoft.AspNetCore.Mvc;
using Users.Application.Dto;
using Users.Application.Interfaces.Services;

namespace Users.API.Controllers;

/// <summary>
/// Manages materialized partner hierarchy (ancestors up to level 10).
/// </summary>
[ApiController]
[Route("api/partner-relations")]
[Produces("application/json")]
public class PartnerRelationsController(IPartnerRelationService partnerRelationService) : ControllerBase
{
    /// <summary>
    /// Sets a direct partner for a user and materializes ancestor rows.
    /// </summary>
    /// <param name="partnerRelationDto">User and partner external ids.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>User external id.</returns>
    /// <response code="200">Relation created.</response>
    /// <response code="400">Validation failed.</response>
    /// <response code="404">User or partner not found.</response>
    /// <response code="409">Relation already exists or cycle detected.</response>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<Guid>> Set(
        [FromBody] CreatePartnerRelationDto partnerRelationDto,
        CancellationToken cancellationToken)
    {
        var result = await partnerRelationService.SetPartnerRelationAsync(partnerRelationDto, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Returns partner ancestor relations for a user (levels 1..10).
    /// </summary>
    /// <param name="userId">User external id.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpGet("user/{userId:guid}")]
    [ProducesResponseType(typeof(List<PartnerRelationDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<PartnerRelationDto>>> GetByUserId(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var relations = await partnerRelationService.GetPartnerRelationsByUserIdAsync(userId, cancellationToken);
        return Ok(relations);
    }

    /// <summary>
    /// Returns referrals where the given user is a partner at any level.
    /// </summary>
    /// <param name="userId">Partner user external id.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpGet("referrals/{userId:guid}")]
    [ProducesResponseType(typeof(List<PartnerRelationDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<PartnerRelationDto>>> GetReferralsByUserId(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var referrals = await partnerRelationService.GetReferralsByUserIdAsync(userId, cancellationToken);
        return Ok(referrals);
    }
}
