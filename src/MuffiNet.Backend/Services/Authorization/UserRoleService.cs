namespace MuffiNet.Backend.Services.Authorization;

using Azure.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public partial class UserRoleService
{
    private readonly GraphServiceClient client;
    private readonly IOptions<ActiveDirectoryConfig> options;
    private ActiveDirectoryConfig config => options.Value;

    public UserRoleService(IOptions<ActiveDirectoryConfig> options)
    {
        this.options = options;

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
                var expected = exception.InnerException is ServiceException innerException
                    && innerException.Error.Details.First().Code == "InvalidUpdate";

                // NOTE: invalid updates are expected, if the User has already been granted the Administrators Role.

                if (! expected)
                {
                    throw new InvalidOperationException("Unexpected Service Error", exception);
                }
            }
        }
    }

    public async Task<IEnumerable<AppRole>> ListAppRoles()
    {
        var app = await client
            .Applications[config.AppRegistrationID]
            .Request()
            .Select(c => new { c.AppRoles }) // optimization
            .GetAsync();

        return app.AppRoles;
    }

    public async Task<IEnumerable<User>> ListUsers()
    {
        var page = await client
            .ServicePrincipals[config.AppID]
            .AppRoleAssignedTo
            .Request()
            .Top(999) // TODO use pagination to fetch ALL records
            .GetAsync();

        return page
            .AsEnumerable()
            .GroupBy(a => a.PrincipalId)
            .Select(group => new User(
                name: group.First().PrincipalDisplayName,
                userID: group.Key!.Value,
                appRoleIDs: group
                    .Where(a => a.AppRoleId != Guid.Empty)
                    .Select(a => a.AppRoleId!.Value)
            ));
    }

    public async Task<Guid> GetAppRoleID(string name)
    {
        var roles = await ListAppRoles();

        var role = roles.FirstOrDefault(role => role.Value == name);

        if (role is null)
        {
            throw new ArgumentException($"No Role with the specified name exists: {name}");
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

    public async Task RevokeUserRoleAssignment(string appRoleAssignmentID)
    {
        await client
            .ServicePrincipals[config.AppID]
            .AppRoleAssignedTo[appRoleAssignmentID]
            .Request()
            .DeleteAsync();
    }
}
