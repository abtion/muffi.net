using Microsoft.Graph;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.UserAdministration.Services;

public interface IGetAppRolesFromAzureIdentity 
{
    public Task<IQueryable<AppRole>> GetAppRoles();
}

public class GetAppRolesFromAzureIdentity : IGetAppRolesFromAzureIdentity
{
    private readonly IConfiguredGraphServiceClient client;

    public GetAppRolesFromAzureIdentity(IConfiguredGraphServiceClient configuredGraphServiceClient)
    {
        this.client = configuredGraphServiceClient;
    }

    public async Task<IQueryable<AppRole>> GetAppRoles()
    {
        var app = await client.Client
            .Applications[client.Options.AppRegistrationObjectId]
            .Request()
            .Select(c => new { c.AppRoles }) // optimization
            .GetAsync();

        return app.AppRoles.AsQueryable();
    }
}