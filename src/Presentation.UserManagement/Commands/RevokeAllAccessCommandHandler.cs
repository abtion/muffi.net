using Domain.Shared;
using Domain.UserAdministration.Repositories;
using Domain.UserAdministration.Services;
using Presentation.UserAdministration.Dtos;

namespace Presentation.UserAdministration.Commands;

public class RevokeAllAccessCommandHandler(IGetUserAppRoleAssignmentsFromAzureIdentity GetUserAppRoleAssignmentsFromAzureIdentity, IRemoveAppRoleFromUser removeAppRoleFromUser) : ICommandHandler<RevokeAllAccessCommand, RevokeAllAccessResponse>
{
    public async Task<RevokeAllAccessResponse> Handle(RevokeAllAccessCommand request, CancellationToken cancellationToken)
    {
        var allAssignments = await GetUserAppRoleAssignmentsFromAzureIdentity.GetUserAppRoleAssignments(request.UserId);

        foreach (var assignment in allAssignments)
        {
            if (assignment.Id is not null)
                await removeAppRoleFromUser.RemoveAppRoleFromUser(assignment.Id, cancellationToken);
        }

        return new();
    }
}
