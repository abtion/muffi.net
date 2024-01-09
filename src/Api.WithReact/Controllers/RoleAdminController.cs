using Domain.UserAdministration.Commands;
using Domain.UserAdministration.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.WithReact.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Administrators")]
public class RoleAdminController : ControllerBase
{
    [HttpGet("roles-and-users")]
    public async Task<ActionResult<LoadUsersAndRolesResponse>> LoadRolesAndUsers(
        [FromServices] LoadUsersAndRolesQueryHandler handler,
        CancellationToken cancellationToken
    )
    {
        return await handler.Handle(new(), cancellationToken);
    }

    [HttpPost("update-user")]
    public async Task<ActionResult<UpdateUserResponse>> UpdateUser(
        [FromServices] UpdateUserCommandHandler handler,
        [FromBody] UpdateUserCommand request,
        CancellationToken cancellationToken
    )
    {
        return await handler.Handle(request, cancellationToken);
    }

    [HttpGet("user-details")]
    public async Task<ActionResult<LoadUserResponse>> GetUserDetails(
        [FromServices] LoadUserQueryHandler handler,
        [FromQuery] string userId,
        CancellationToken cancellationToken
    )
    {
        return await handler.Handle(new(userId), cancellationToken);
    }

    [HttpPost("revoke-access")]
    public async Task<ActionResult<RevokeAllAccessResponse>> RevokeAccess(
        [FromServices] RevokeAllAccessCommandHandler handler,
        [FromQuery] string userId,
        CancellationToken cancellationToken
    )
    {
        return await handler.Handle(new(userId), cancellationToken);
    }
}
