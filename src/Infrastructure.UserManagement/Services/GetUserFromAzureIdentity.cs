using Domain.UserAdministration.Exceptions;
using Microsoft.Graph.Models;
using System.Threading.Tasks;

namespace Domain.UserAdministration.Services;

public interface IGetUserFromAzureIdentity
{
    public Task<User> GetUser(string userId);
}

public class GetUserFromAzureIdentity : IGetUserFromAzureIdentity
{
    private readonly IConfiguredGraphServiceClient client;

    public GetUserFromAzureIdentity(IConfiguredGraphServiceClient configuredGraphServiceClient)
    {
        client = configuredGraphServiceClient;
    }

    public async Task<User> GetUser(string userId)
    {
        var azureUser = await client.Client.Users[userId].GetAsync(requestConfiguration =>
        {
            requestConfiguration.QueryParameters.Select = ["mail"];
        });

        if (azureUser is null)
            throw new AzureUserNotFoundException(userId);

        return azureUser;
    }
}
