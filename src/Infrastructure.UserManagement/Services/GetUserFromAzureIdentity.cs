using Domain.UserAdministration.Exceptions;
using Microsoft.Graph.Models;

namespace Domain.UserAdministration.Services;

public interface IGetUserFromAzureIdentity
{
    public Task<User> GetUser(string userId);
}

public class GetUserFromAzureIdentity(IConfiguredGraphServiceClient ConfiguredGraphServiceClient) : IGetUserFromAzureIdentity
{
    public async Task<User> GetUser(string userId)
    {
        var azureUser = await ConfiguredGraphServiceClient.Client.Users[userId].GetAsync(requestConfiguration =>
        {
            requestConfiguration.QueryParameters.Select = ["mail"];
        });

        return azureUser is null ? throw new AzureUserNotFoundException(userId) : azureUser;
    }
}
