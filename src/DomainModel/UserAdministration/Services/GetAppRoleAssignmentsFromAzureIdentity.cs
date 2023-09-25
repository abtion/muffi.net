using Microsoft.Graph.Models;
using System.Collections.Generic;
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
        var azureRoleAssignments = await client.Client.ServicePrincipals[
            client.Options.EnterpriseApplicationObjectId
        ].AppRoleAssignedTo.GetAsync(requestConfiguration =>
        {
            requestConfiguration.QueryParameters.Top = 999;
            requestConfiguration.QueryParameters.Select = new string[]
            {
                "principalId",
                "principalDisplayName",
                "appRoleId"
            };
        });

        if (azureRoleAssignments is not null && azureRoleAssignments.Value is not null)
            return azureRoleAssignments.Value.AsQueryable();

        var result = new List<AppRoleAssignment>();
        return result.AsQueryable();
    }

    public async Task<IQueryable<AppRoleAssignment>> GetAppRoleAssignmentsForUser(string userId)
    {
        var roleAssignments = await client.Client.Users[
            userId
        ].AppRoleAssignments.GetAsync(requestConfiguration =>
        {
            requestConfiguration.QueryParameters.Top = 999;
            requestConfiguration.QueryParameters.Filter =
                $"resourceId eq {client.Options.EnterpriseApplicationObjectId}";
        });

        if (roleAssignments is not null && roleAssignments.Value is not null)
            return roleAssignments.Value.AsQueryable();

        var result = new List<AppRoleAssignment>();
        return result.AsQueryable();
    }
}
