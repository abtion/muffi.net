using DomainModel.Shared;
using DomainModel.UserAdministration.Services;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DomainModel.UserAdministration.Queries;

public class LoadUsersAndRolesQueryHandler
    : IQueryHandler<LoadUsersAndRolesQuery, LoadUsersAndRolesResponse>
{
    private readonly IGetAppRolesFromAzureIdentity appRoles;
    private readonly IGetAppRoleAssignmentsFromAzureIdentity appRoleAssignments;

    public LoadUsersAndRolesQueryHandler(
        IGetAppRolesFromAzureIdentity appRoles,
        IGetAppRoleAssignmentsFromAzureIdentity appRoleAssignments
    )
    {
        this.appRoles = appRoles;
        this.appRoleAssignments = appRoleAssignments;
    }

    public async Task<LoadUsersAndRolesResponse> Handle(
        LoadUsersAndRolesQuery request,
        CancellationToken cancellationToken
    )
    {
        var roles = (await appRoles.GetAppRoles()).Select(
            p =>
                new LoadUsersAndRolesResponse.LoadedRole(
                    p.Id.GetValueOrDefault(),
                    p.DisplayName ?? String.Empty
                )
        );

        var roleAssignments = await appRoleAssignments.GetAppRoleAssignments();

        var users = roleAssignments
            .GroupBy(a => a.PrincipalId)
            .Select(
                group =>
                    new LoadUsersAndRolesResponse.LoadedUser(
                        group.First().PrincipalDisplayName ?? String.Empty,
                        group.Key!.Value,
                        group.Where(a => a.AppRoleId != Guid.Empty).Select(a => a.AppRoleId!.Value)
                    )
            );

        return new LoadUsersAndRolesResponse(roles, users);
    }
}
