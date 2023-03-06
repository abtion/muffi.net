namespace DomainModel.Services.Authorization;

using Azure.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class UserRoleService {
    private readonly GraphServiceClient client;
    private readonly IOptions<ActiveDirectoryConfig> options;
    private ActiveDirectoryConfig config => options.Value;

    // TODO extract an interface and add a mock implementation for integration testing

    public UserRoleService(IOptions<ActiveDirectoryConfig> options) {
        this.options = options;

        string[] scopes = { "https://graph.microsoft.com/.default" };

        var clientSecretCredential = new ClientSecretCredential(
            config.DirectoryTenantID,
            config.AppClientID,
            config.AppClientSecret
        );

        client = new GraphServiceClient(clientSecretCredential, scopes);

        if (config.AdminUserID is not null) {
            TryAssignUserRole(config.AdminUserID, config.BaseRoleID);
            TryAssignUserRole(config.AdminUserID, config.AdminRoleID);
        }
    }

    public async Task<IEnumerable<AppRole>> ListAppRoles() {
        var app = await client
            .Applications[config.AppRegistrationID]
            .Request()
            .Select(c => new { c.AppRoles }) // optimization
            .GetAsync();

        return app.AppRoles;
    }

    public async Task<IEnumerable<User>> ListUsers() {
        var page = await client
            .ServicePrincipals[config.AppID]
            .AppRoleAssignedTo
            .Request()
            .Select(a => new { a.PrincipalId, a.PrincipalDisplayName, a.AppRoleId }) // optimization
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

    public async Task<Guid> GetAppRoleID(string name) {
        var roles = await ListAppRoles();

        var role = roles.FirstOrDefault(role => role.Value == name);

        if (role is null) {
            throw new ArgumentException($"No Role with the specified name exists: {name}");
        }

        return role.Id!.Value;
    }

    public async Task AssignUserRole(string userID, string appRoleID) {
        var assignment = new AppRoleAssignment {
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

    public async Task RevokeUserRoleAssignment(string appRoleAssignmentID) {
        await client
            .ServicePrincipals[config.AppID]
            .AppRoleAssignedTo[appRoleAssignmentID]
            .Request()
            .DeleteAsync();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1851:Possible multiple enumerations of 'IEnumerable' collection", Justification = "<Pending>")]
    public async Task UpdateUser(User user) {
        // Update User details:

        var update = new Microsoft.Graph.User {
            DisplayName = user.Name,
        };

        await client
            .Users[user.UserID.ToString()]
            .Request()
            .UpdateAsync(update);

        // Fetch existing App Role Assignments assigned to this User:

        var currentAssignments = (await GetAssignments(user.UserID.ToString()))
            .Where(a => a.AppRoleId != Guid.Empty); // ignoring the default App Role

        // TODO add test coverage: this area is dangerous and fragile

        // Add any new App Role Assignments not already assigned to this User:

        if (currentAssignments is not null) {
            foreach (var roleID in user.AppRoleIDs) {
                if (!currentAssignments.Any(a => a.AppRoleId == roleID)) {
                    await AssignUserRole(user.UserID.ToString(), roleID.ToString());
                }
            }

            // Revoke any old App Role Assignments no longer assigned to this User:

            foreach (var currentAssignment in currentAssignments) {
                if (!user.AppRoleIDs.Any(id => currentAssignment.AppRoleId == id)) {
                    await RevokeUserRoleAssignment(currentAssignment.Id);
                }
            }
        }
    }

    public async Task<UserDetails> GetUserDetails(string userID) {
        var user = await client
            .Users[userID]
            .Request()
            .Select(u => new { u.Mail }) // optimization
            .GetAsync();

        return new UserDetails(user.Mail);
    }

    public async Task RevokeAccess(string userID) {
        var allAssignments = await GetAssignments(userID);

        foreach (var assignment in allAssignments) {
            await RevokeUserRoleAssignment(assignment.Id);
        }
    }

    private async Task<IUserAppRoleAssignmentsCollectionPage> GetAssignments(string userID) {
        return await client
            .Users[userID]
            .AppRoleAssignments
            .Request()
            .Filter($"resourceId eq {config.AppID}")
            .GetAsync();
    }

    // If we make this async and await AssingUserRole, and then call this with .Wait(), //
    // the AssignUserRole throws an unhandled exception.. //
    private void TryAssignUserRole(string userID, string roleID) {
        try {
            AssignUserRole(userID, roleID).Wait();
        }
        catch (Exception exception) {
            var expected = exception.InnerException is not null
                && exception.InnerException is ServiceException innerException
                && innerException.Error.Details is not null
                && innerException.Error.Details.First().Code == "InvalidUpdate";

            // NOTE: invalid updates are expected, if the User has already been granted the Administrators Role.
            if (!expected) {
                throw new InvalidOperationException("Unexpected Service Error", exception);
            }
        }
    }
}
