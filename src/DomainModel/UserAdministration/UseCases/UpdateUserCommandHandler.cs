using Domain.Shared;
using Domain.UserAdministration.Services;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.UserAdministration.Commands;

public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, UpdateUserResponse>
{
    private readonly IAddUserAppRoleAssignmentToAzureIdentity addUserAppRoleAssignmentToAzureIdentity;
    private readonly IDeleteUserAppRoleAssignmentFromAzureIdentity deleteUserAppRoleAssignmentFromAzureIdentity;
    private readonly IGetAppRoleAssignmentsFromAzureIdentity getAppRoleAssignmentsFromAzureIdentity;
    private readonly IUpdateUserInAzureIdentity updateUserInAzureIdentity;

    public UpdateUserCommandHandler(
        IAddUserAppRoleAssignmentToAzureIdentity addUserAppRoleAssignmentToAzureIdentity,
        IDeleteUserAppRoleAssignmentFromAzureIdentity deleteUserAppRoleAssignmentFromAzureIdentity,
        IGetAppRoleAssignmentsFromAzureIdentity getAppRoleAssignmentsFromAzureIdentity,
        IUpdateUserInAzureIdentity updateUserInAzureIdentity
    )
    {
        this.addUserAppRoleAssignmentToAzureIdentity = addUserAppRoleAssignmentToAzureIdentity;
        this.deleteUserAppRoleAssignmentFromAzureIdentity =
            deleteUserAppRoleAssignmentFromAzureIdentity;
        this.getAppRoleAssignmentsFromAzureIdentity = getAppRoleAssignmentsFromAzureIdentity;
        this.updateUserInAzureIdentity = updateUserInAzureIdentity;
    }

    public async Task<UpdateUserResponse> Handle(
        UpdateUserCommand request,
        CancellationToken cancellationToken
    )
    {
        // Update User details:
        var update = new Microsoft.Graph.Models.User
        {
            Id = request.UserId.ToString(),
            DisplayName = request.Name,
        };

        await updateUserInAzureIdentity.UpdateUser(update);

        // Fetch existing App Role Assignments assigned to this User:
        var allCurrentAssignments =
            await getAppRoleAssignmentsFromAzureIdentity.GetAppRoleAssignmentsForUser(
                request.UserId.ToString()
            );

        // ignoring the default App Role
        var currentAssignments = allCurrentAssignments.Where(a => a.AppRoleId != Guid.Empty);

        // Add any new App Role Assignments not already assigned to this User:
        if (currentAssignments is not null)
        {
            foreach (var roleID in request.AppRoleIds)
            {
                if (!currentAssignments.Any(a => a.AppRoleId == roleID))
                    await addUserAppRoleAssignmentToAzureIdentity.AddUserAppRoleAssignment(
                        request.UserId.ToString(),
                        roleID.ToString()
                    );
            }

            // Revoke any old App Role Assignments no longer assigned to this User:
            foreach (var currentAssignment in currentAssignments)
            {
                if (currentAssignment is not null && currentAssignment.Id is not null)
                    if (!request.AppRoleIds.Any(id => currentAssignment.AppRoleId == id))
                        await deleteUserAppRoleAssignmentFromAzureIdentity.DeleteAppRoleAssignment(
                            currentAssignment.Id
                        );
            }
        }

        return new();
    }
}
