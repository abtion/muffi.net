namespace MuffiNet.FrontendReact.Authorization;

using Azure.Identity;
using Microsoft.Graph;

public class UserRoleService
{
    private readonly GraphServiceClient client;
    private readonly ActiveDirectoryConfig config;

    public UserRoleService(ActiveDirectoryConfig config)
    {
        this.config = config;

        string[] scopes = { "https://graph.microsoft.com/.default" };

        var clientSecretCredential = new ClientSecretCredential(
            config.DirectoryTenantID,
            config.AppClientID,
            config.AppClientSecret
        );

        client = new GraphServiceClient(clientSecretCredential, scopes);

        if (config.AdminUserID is not null)
        {
            try
            {
                AssignUserRole(config.AdminUserID, config.AdminRoleID).Wait();
            }
            catch (Exception exception)
            {
                var expected = false;

                if (exception.InnerException is ServiceException innerException)
                {
                    if (innerException.Error.Details.First().Code == "InvalidUpdate")
                    {
                        expected = true; // NOTE: invalid updates are expected, if the User has already been granted the Administrators Role.
                    }
                }

                if (! expected)
                {
                    throw new Exception("Unexpected Service Error", exception);
                }
            }
        }
    }

    // TODO add .Top(999) to all queries for the time being (in lieu of paging and/or search capability)
    //      https://docs.microsoft.com/en-us/graph/sdks/paging?tabs=csharp
    //      no idea how many records these are fetching by default.
    //      the Top method doesn't seem to exist - might not be supported in the current version?

    public async Task<IEnumerable<AppRole>> ListAppRoles()
    {
        var app = await client
            .Applications[config.AppRegistrationID]
            .Request()
            .Select(c => new { c.AppRoles }) // optimization
            .GetAsync();

        return app.AppRoles;
    }

    public async Task<Guid> GetAppRoleID(string name)
    {
        var roles = await ListAppRoles();

        var role = roles.FirstOrDefault(role => role.Value == name);

        if (role is null)
        {
            throw new Exception($"Role does not exist: {name}");
        }

        return role.Id!.Value;
    }

    public async Task AssignUserRole(string userID, string appRoleID)
    {
        var assignment = new AppRoleAssignment
        {
            PrincipalId = Guid.Parse(userID),
            ResourceId = Guid.Parse(config.AppID),
            AppRoleId = Guid.Parse(appRoleID),
        };

        await client
            .ServicePrincipals[config.AppID]
            .AppRoleAssignedTo
            .Request()
            .AddAsync(assignment);
    }

    public async Task ListRoleAssignments()
    {
        var appRoleAssignments = await client
            .ServicePrincipals[config.AppID]
            .AppRoleAssignments
            .Request()
            .GetAsync();


        // ...
    }

    public async Task RevokeUserRoleAssignment(string appRoleAssignmentID)
    {
        await client
            .ServicePrincipals[config.AppID]
            .AppRoleAssignedTo[appRoleAssignmentID]
            .Request()
            .DeleteAsync();
    }
}
