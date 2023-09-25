using DomainModel.Shared;
using DomainModel.UserAdministration.Services;
using System.Threading;
using System.Threading.Tasks;

namespace DomainModel.UserAdministration.Commands;

public class RevokeAllAccessCommandHandler
    : ICommandHandler<RevokeAllAccessCommand, RevokeAllAccessResponse>
{
    private readonly IGetUserAppRoleAssignmentsFromAzureIdentity getUserAppRoleAssignmentsFromAzureIdentity;
    private readonly IDeleteUserAppRoleAssignmentFromAzureIdentity deleteUserAppRoleAssignmentFromAzureIdentity;

    public RevokeAllAccessCommandHandler(
        IGetUserAppRoleAssignmentsFromAzureIdentity getUserAppRoleAssignmentsFromAzureIdentity,
        IDeleteUserAppRoleAssignmentFromAzureIdentity deleteUserAppRoleAssignmentFromAzureIdentity
    )
    {
        this.getUserAppRoleAssignmentsFromAzureIdentity =
            getUserAppRoleAssignmentsFromAzureIdentity;
        this.deleteUserAppRoleAssignmentFromAzureIdentity =
            deleteUserAppRoleAssignmentFromAzureIdentity;
    }

    public async Task<RevokeAllAccessResponse> Handle(
        RevokeAllAccessCommand request,
        CancellationToken cancellationToken
    )
    {
        var allAssignments =
            await getUserAppRoleAssignmentsFromAzureIdentity.GetUserAppRoleAssignments(
                request.UserId
            );

        foreach (var assignment in allAssignments)
        {
            if (assignment.Id is not null)
                await deleteUserAppRoleAssignmentFromAzureIdentity.DeleteAppRoleAssignment(
                    assignment.Id
                );
        }

        return new();
    }
}
