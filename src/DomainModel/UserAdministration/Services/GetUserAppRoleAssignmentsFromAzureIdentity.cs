using Microsoft.Graph.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.UserAdministration.Services;

public interface IGetUserAppRoleAssignmentsFromAzureIdentity
{
    public Task<IQueryable<AppRoleAssignment>> GetUserAppRoleAssignments(string userId);
}

public class GetUserAppRoleAssignmentsFromAzureIdentity
    : IGetUserAppRoleAssignmentsFromAzureIdentity
{
    private readonly IConfiguredGraphServiceClient client;

    public GetUserAppRoleAssignmentsFromAzureIdentity(
        IConfiguredGraphServiceClient configuredGraphServiceClient
    )
    {
        client = configuredGraphServiceClient;
    }

    public async Task<IQueryable<AppRoleAssignment>> GetUserAppRoleAssignments(string userId)
    {
        var query = await client.Client.Users[
            userId
        ].AppRoleAssignments.GetAsync(requestConfiguration =>
        {
            requestConfiguration.QueryParameters.Top = 999;
            requestConfiguration.QueryParameters.Filter =
                $"resourceId eq {client.Options.EnterpriseApplicationObjectId}";
        });

        if (query is not null && query.Value is not null)
            query.Value.AsQueryable();

        var result = new List<AppRoleAssignment>();
        return result.AsQueryable();
    }
}
