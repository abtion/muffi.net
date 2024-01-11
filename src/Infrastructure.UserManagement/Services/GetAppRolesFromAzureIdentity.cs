using Domain.UserAdministration.Exceptions;
using Microsoft.Graph.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.UserAdministration.Services;

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
        var azureApp = await client.Client.Applications[
            client.Options.AppRegistrationObjectId
        ].GetAsync(requestConfiguration =>
        {
            requestConfiguration.QueryParameters.Select = ["appRoles"];
        });

        if (azureApp is null)
            throw new AzureApplicationNotFoundException();

        if (azureApp.AppRoles is not null)
            return azureApp.AppRoles.AsQueryable();

        var result = new List<AppRole>();
        return result.AsQueryable();
    }
}
