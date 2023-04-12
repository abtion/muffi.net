using Microsoft.Graph;
using System;
using System.Threading.Tasks;

namespace DomainModel.UserAdministration.Services;

public interface IAddUserAppRoleAssignmentToAzureIdentity 
{
    public Task AddUserAppRoleAssignment(string userId, string appRoleId);
}

public class AddUserAppRoleAssignmentToAzureIdentity : IAddUserAppRoleAssignmentToAzureIdentity
{
    private readonly IConfiguredGraphServiceClient client;

    public AddUserAppRoleAssignmentToAzureIdentity(IConfiguredGraphServiceClient configuredGraphServiceClient)
    {
        this.client = configuredGraphServiceClient;
    }

    public async Task AddUserAppRoleAssignment(string userId, string appRoleId) 
    {
        var assignment = new AppRoleAssignment
        {
            PrincipalId = Guid.Parse(userId),
            ResourceId = Guid.Parse(client.Options.EnterpriseApplicationObjectId),
            AppRoleId = Guid.Parse(appRoleId),
        };

        await client.Client
            .ServicePrincipals[client.Options.EnterpriseApplicationObjectId]
            .AppRoleAssignedTo
            .Request()
            .AddAsync(assignment);
    }
}