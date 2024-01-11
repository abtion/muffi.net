using Domain.Shared;
using Domain.UserAdministration.Repositories;
using Domain.UserAdministration.Services;
using Presentation.UserAdministration.Dtos;

namespace Presentation.UserAdministration.Commands;

public class UpdateUserCommandHandler(IAssignAppRoleToUser assignAppRoleToUser, IRemoveAppRoleFromUser removeAppRoleFromUser, IGetAppRoleAssignmentsFromAzureIdentity GetAppRoleAssignmentsFromAzureIdentity, IUpdateUserInAzureIdentity UpdateUserInAzureIdentity) : ICommandHandler<UpdateUserCommand, UpdateUserResponse>
{
    public async Task<UpdateUserResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        // Update User details:
        var update = new Microsoft.Graph.Models.User
        {
            Id = request.UserId.ToString(),
            DisplayName = request.Name,
        };

        await UpdateUserInAzureIdentity.UpdateUser(update);

        // Fetch existing App Role Assignments assigned to this User:
        var allCurrentAssignments = await GetAppRoleAssignmentsFromAzureIdentity.GetAppRoleAssignmentsForUser(request.UserId.ToString());

        // ignoring the default App Role
        var currentAssignments = allCurrentAssignments.Where(a => a.AppRoleId != Guid.Empty);

        // Add any new App Role Assignments not already assigned to this User:
        if (currentAssignments is not null)
        {
            foreach (var roleID in request.AppRoleIds)
            {
                if (!currentAssignments.Any(a => a.AppRoleId == roleID))
                    await assignAppRoleToUser.AssignAppRoleToUser(request.UserId.ToString(), roleID.ToString(), cancellationToken);
            }

            // Revoke any old App Role Assignments no longer assigned to this User:
            foreach (var currentAssignment in currentAssignments)
            {
                if (currentAssignment is not null && currentAssignment.Id is not null)
                    if (!request.AppRoleIds.Any(id => currentAssignment.AppRoleId == id))
                        await removeAppRoleFromUser.RemoveAppRoleFromUser(currentAssignment.Id, cancellationToken);
            }
        }

        return new();
    }
}
