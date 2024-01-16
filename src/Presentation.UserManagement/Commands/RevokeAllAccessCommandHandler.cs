using Domain.Shared;
using Domain.UserAdministration.Repositories;
using Domain.UserAdministration.Services;
using Presentation.UserAdministration.Dtos;

namespace Presentation.UserAdministration.Commands;

public class RevokeAllAccessCommandHandler(IGetUserAppRoleAssignments GetUserAppRoleAssignments , IRemoveAppRoleFromUser removeAppRoleFromUser) : ICommandHandler<RevokeAllAccessCommand, RevokeAllAccessResponse>
{
    public async Task<RevokeAllAccessResponse> Handle(RevokeAllAccessCommand request, CancellationToken cancellationToken)
    {
        var allAssignments = await GetUserAppRoleAssignments.GetUserAppRoleAssignments(request.UserId, cancellationToken);

        foreach (var assignmentId in allAssignments)
        {
            if (assignmentId is not null)
                await removeAppRoleFromUser.RemoveAppRoleFromUser(assignmentId, cancellationToken);
        }

        return new();
    }
}
