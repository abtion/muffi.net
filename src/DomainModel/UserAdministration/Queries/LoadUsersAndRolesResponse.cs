using System;
using System.Collections.Generic;

namespace Domain.UserAdministration.Queries;

public class LoadUsersAndRolesResponse
{
    public LoadUsersAndRolesResponse(IEnumerable<LoadedRole> roles, IEnumerable<LoadedUser> users)
    {
        Roles = roles;
        Users = users;
    }

    public IEnumerable<LoadedRole> Roles { get; }

    public IEnumerable<LoadedUser> Users { get; }

    public record LoadedRole(Guid Id, string Name);

    public record LoadedUser(string Name, Guid UserId, IEnumerable<Guid> AppRoleIds);
}
