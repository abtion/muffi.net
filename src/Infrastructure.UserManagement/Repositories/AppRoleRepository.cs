using Domain.UserAdministration.Entities;
using Domain.UserAdministration.Exceptions;
using Domain.UserAdministration.Repositories;
using Domain.UserAdministration.Services;

namespace Infrastructure.UserManagement.Repositories;

public class AppRoleRepository(IConfiguredGraphServiceClient ConfiguredGraphServiceClient) : IGetAppRoles
{
    public async Task<List<AppRoleEntity>> GetAppRoles()
    {
        var azureApp = await ConfiguredGraphServiceClient.Client.Applications[ConfiguredGraphServiceClient.Options.AppRegistrationObjectId].GetAsync(requestConfiguration =>
        {
            requestConfiguration.QueryParameters.Select = ["appRoles"];
        });

        if (azureApp is null)
            throw new AzureApplicationNotFoundException();

        if (azureApp.AppRoles is null)
            return [];

        return azureApp.AppRoles.Select(p => new AppRoleEntity(p.Id, p.DisplayName)).ToList();
    }
}
