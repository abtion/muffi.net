using Microsoft.Graph;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.UserAdministration.Services;

public interface IGetAppRoleAssignmentsFromAzureIdentity
{
    public Task<IQueryable<AppRoleAssignment>> GetAppRoleAssignments();

    public Task<IQueryable<AppRoleAssignment>> GetAppRoleAssignmentsForUser(string userId);
}

public class GetAppRoleAssignmentsFromAzureIdentity : IGetAppRoleAssignmentsFromAzureIdentity
{
    private readonly IConfiguredGraphServiceClient client;

    public GetAppRoleAssignmentsFromAzureIdentity(
        IConfiguredGraphServiceClient configuredGraphServiceClient
    )
    {
        this.client = configuredGraphServiceClient;
    }

    public async Task<IQueryable<AppRoleAssignment>> GetAppRoleAssignments()
    {
        var roleAssignments = await client.Client.ServicePrincipals[
            client.Options.EnterpriseApplicationObjectId
        ].AppRoleAssignedTo
            .Request()
            .Select(
                a =>
                    new
                    {
                        a.PrincipalId,
                        a.PrincipalDisplayName,
                        a.AppRoleId
                    }
            )
            .Top(999) // TODO use pagination to fetch ALL records
            .GetAsync();

        return roleAssignments.AsQueryable();
    }

    public async Task<IQueryable<AppRoleAssignment>> GetAppRoleAssignmentsForUser(string userId)
    {
        var roleAssignments = await client.Client.Users[userId].AppRoleAssignments
            .Request()
            .Filter($"resourceId eq {client.Options.EnterpriseApplicationObjectId}")
            .GetAsync();

        return roleAssignments.AsQueryable();
    }
}
