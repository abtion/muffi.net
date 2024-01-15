using Domain.UserAdministration.Repositories;
using Domain.UserAdministration.Services;
using Microsoft.Graph.Models;

namespace Infrastructure.UserManagement.Repositories;

internal class UserRepository(IConfiguredGraphServiceClient GraphClient) : IUpdateUserDetails, IGetUserAppRoleAssignments
{
    public async Task<List<string?>> GetUserAppRoleAssignments(string userId, CancellationToken cancellationToken)
    {
        var query = await GraphClient.Client.Users[userId].AppRoleAssignments.GetAsync(requestConfiguration =>
        {
            requestConfiguration.QueryParameters.Top = 999;
            requestConfiguration.QueryParameters.Filter = $"resourceId eq {GraphClient.Options.EnterpriseApplicationObjectId}";
        }, cancellationToken);

        if (query is not null && query.Value is not null && query.Value.Count != 0)
            return query.Value.Select(p => p.Id).ToList();

        return [];
    }

    public async Task UpdateUser(string id, string DisplayName, CancellationToken cancellationToken)
    {
        var update = new User
        {
            Id = id.ToString(),
            DisplayName = DisplayName,
        };

        await GraphClient.Client.Users[id].PatchAsync(update, cancellationToken: cancellationToken);
    }
}
