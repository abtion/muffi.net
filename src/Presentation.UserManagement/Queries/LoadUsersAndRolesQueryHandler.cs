using Domain.Shared;
using Domain.UserAdministration.Services;
using Presentation.UserAdministration.Dtos;

namespace Presentation.UserAdministration.Queries;

public class LoadUsersAndRolesQueryHandler(IGetAppRolesFromAzureIdentity AppRoles, IGetAppRoleAssignmentsFromAzureIdentity AppRoleAssignments) : IQueryHandler<LoadUsersAndRolesQuery, LoadUsersAndRolesResponse>
{
    public async Task<LoadUsersAndRolesResponse> Handle(
        LoadUsersAndRolesQuery request,
        CancellationToken cancellationToken
    )
    {
        var roles = (await AppRoles.GetAppRoles()).Select(
            p =>
                new LoadUsersAndRolesResponse.LoadedRole(
                    p.Id.GetValueOrDefault(),
                    p.DisplayName ?? String.Empty
                )
        );

        var roleAssignments = await AppRoleAssignments.GetAppRoleAssignments();

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
