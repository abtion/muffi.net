using Microsoft.Graph;
using System.Threading.Tasks;

namespace DomainModel.UserAdministration.Services;

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
        return await client.Client
            .Users[userId]
            .Request()
            .Select(u => new { u.Mail }) // optimization
            .GetAsync();
    }
}