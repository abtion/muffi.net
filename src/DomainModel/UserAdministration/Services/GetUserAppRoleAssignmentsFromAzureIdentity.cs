using Microsoft.Graph;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.UserAdministration.Services;

public interface IGetUserAppRoleAssignmentsFromAzureIdentity
{
    public Task<IQueryable<AppRoleAssignment>> GetUserAppRoleAssignments(string userId);
}

public class GetUserAppRoleAssignmentsFromAzureIdentity : IGetUserAppRoleAssignmentsFromAzureIdentity
{
    private readonly IConfiguredGraphServiceClient client;

    public GetUserAppRoleAssignmentsFromAzureIdentity(IConfiguredGraphServiceClient configuredGraphServiceClient)
    {
        client = configuredGraphServiceClient;
    }

    public async Task<IQueryable<AppRoleAssignment>> GetUserAppRoleAssignments(string userId)
    {
        var query = await client.Client
            .Users[userId]
            .AppRoleAssignments
            .Request()
            .Filter($"resourceId eq {client.Options.EnterpriseApplicationObjectId}")
            .GetAsync();

        return query.AsQueryable();
    }
}
