using Microsoft.Graph.Models;

namespace Domain.UserAdministration.Services;

public interface IGetUserAppRoleAssignmentsFromAzureIdentity
{
    public Task<IQueryable<AppRoleAssignment>> GetUserAppRoleAssignments(string userId);
}

public class GetUserAppRoleAssignmentsFromAzureIdentity(IConfiguredGraphServiceClient ConfiguredGraphServiceClient) : IGetUserAppRoleAssignmentsFromAzureIdentity
{
    public async Task<IQueryable<AppRoleAssignment>> GetUserAppRoleAssignments(string userId)
    {
        var query = await ConfiguredGraphServiceClient.Client.Users[userId].AppRoleAssignments.GetAsync(requestConfiguration =>
        {
            requestConfiguration.QueryParameters.Top = 999;
            requestConfiguration.QueryParameters.Filter = $"resourceId eq {ConfiguredGraphServiceClient.Options.EnterpriseApplicationObjectId}";
        });

        if (query is not null && query.Value is not null)
            query.Value.AsQueryable();

        var result = new List<AppRoleAssignment>();

        return result.AsQueryable();
    }
}