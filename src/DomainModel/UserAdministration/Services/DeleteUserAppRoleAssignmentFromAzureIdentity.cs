using System.Threading.Tasks;

namespace DomainModel.UserAdministration.Services;

public interface IDeleteUserAppRoleAssignmentFromAzureIdentity
{
    public Task DeleteAppRoleAssignment(string appRoleAssignmentId);
}

public class DeleteUserAppRoleAssignmentFromAzureIdentity : IDeleteUserAppRoleAssignmentFromAzureIdentity
{
    private readonly IConfiguredGraphServiceClient client;

    public DeleteUserAppRoleAssignmentFromAzureIdentity(IConfiguredGraphServiceClient configuredGraphServiceClient)
    {
        this.client = configuredGraphServiceClient;
    }

    public async Task DeleteAppRoleAssignment(string appRoleAssignmentId)
    {
        await client.Client
            .ServicePrincipals[client.Options.EnterpriseApplicationObjectId]
            .AppRoleAssignedTo[appRoleAssignmentId]
            .Request()
            .DeleteAsync();
    }
}