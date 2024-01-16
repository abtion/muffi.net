using Domain.Shared;
using Domain.UserAdministration.Repositories;
using Presentation.UserAdministration.Dtos;

namespace Presentation.UserAdministration.Queries;

public class LoadUsersAndRolesQueryHandler(IGetAppRoles GetAppRoles, IGetAppRoleAssigment GetAppRoleAssigment) : IQueryHandler<LoadUsersAndRolesQuery, LoadUsersAndRolesResponse>
{
    public async Task<LoadUsersAndRolesResponse> Handle(LoadUsersAndRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = (await GetAppRoles.GetAppRoles()).Select(
            p =>
                new LoadUsersAndRolesResponse.LoadedRole(
                    p.Id.GetValueOrDefault(),
                    p.DisplayName ?? String.Empty
                )
        );

        var roleAssignments = await GetAppRoleAssigment.GetAppRoleAssignments(cancellationToken);

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
