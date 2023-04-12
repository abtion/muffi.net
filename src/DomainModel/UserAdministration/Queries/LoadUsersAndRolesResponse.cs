using System;
using System.Collections.Generic;

namespace DomainModel.UserAdministration.Queries;

public class LoadUsersAndRolesResponse
{
    public LoadUsersAndRolesResponse(
        IEnumerable<Role> roles,
        IEnumerable<User> users) 
    {
        Roles = roles;
        Users = users;
    }

    public IEnumerable<Role> Roles { get; }
    
    public IEnumerable<User> Users { get; }

    public record Role(Guid Id, string Name);

    public record User(string Name, Guid UserId, IEnumerable<Guid> AppRoleIds);
}
