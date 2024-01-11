using Domain.UserAdministration.Repositories;
using Domain.UserAdministration.Services;
using Microsoft.Graph.Models;

namespace Presentation.UserManagement.Repositories;

internal class AppRoleAssignmentRepository(IConfiguredGraphServiceClient ConfiguredGraphServiceClient) : IAssignAppRoleToUser, IRemoveAppRoleFromUser
{
    public async Task AssignAppRoleToUser(string userId, string appRoleId, CancellationToken cancellationToken)
    {
        var enterpriseApplicationObjectId = ConfiguredGraphServiceClient.Options.EnterpriseApplicationObjectId;

        var assignment = new AppRoleAssignment
        {
            PrincipalId = Guid.Parse(userId),
            ResourceId = Guid.Parse(enterpriseApplicationObjectId),
            AppRoleId = Guid.Parse(appRoleId),
        };

        await ConfiguredGraphServiceClient.Client.ServicePrincipals[enterpriseApplicationObjectId].AppRoleAssignedTo.PostAsync(assignment, cancellationToken: cancellationToken);
    }

    public async Task RemoveAppRoleFromUser(string appRoleAssignmentId, CancellationToken cancellationToken)
    {
        var graphClient = ConfiguredGraphServiceClient.Client;
        var enterpriseApplicationObjectId = ConfiguredGraphServiceClient.Options.EnterpriseApplicationObjectId;

        await graphClient.ServicePrincipals[enterpriseApplicationObjectId].AppRoleAssignedTo[appRoleAssignmentId].DeleteAsync(cancellationToken: cancellationToken);
    }
}
