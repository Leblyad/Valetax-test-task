using Microsoft.AspNetCore.Mvc;
using Users.Application.Dto;
using Users.Application.Interfaces.Services;

namespace Users.API.Controllers;

/// <summary>
/// Manages users. Creating a user enqueues wallet creation via outbox.
/// </summary>
[ApiController]
[Route("api/users")]
[Produces("application/json")]
public class UsersController(IUserService userService) : ControllerBase
{
    /// <summary>
    /// Creates a user (idempotent by ExternalId) and schedules wallet creation.
    /// </summary>
    /// <param name="createUserDto">User payload including commission SchemaType for the wallet.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>User external id.</returns>
    /// <response code="201">User created or already existed.</response>
    /// <response code="400">Validation failed.</response>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Guid>> Create(
        [FromBody] CreateUserDto createUserDto,
        CancellationToken cancellationToken)
    {
        var externalId = await userService.CreateUserAsync(createUserDto, cancellationToken);
        return CreatedAtAction(nameof(GetByExternalId), new { externalId }, externalId);
    }

    /// <summary>
    /// Returns a user with distinct partners and referrals.
    /// </summary>
    /// <param name="externalId">User external id.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="200">User found.</response>
    /// <response code="404">User not found.</response>
    [HttpGet("{externalId:guid}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> GetByExternalId(
        Guid externalId,
        CancellationToken cancellationToken)
    {
        var user = await userService.GetUserAsync(externalId, cancellationToken);
        return Ok(user);
    }
}
