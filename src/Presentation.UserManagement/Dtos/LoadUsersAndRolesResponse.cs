namespace Presentation.UserAdministration.Dtos;

public class LoadUsersAndRolesResponse(IEnumerable<LoadUsersAndRolesResponse.LoadedRole> roles, IEnumerable<LoadUsersAndRolesResponse.LoadedUser> users)
{
    public IEnumerable<LoadedRole> Roles { get; } = roles;

    public IEnumerable<LoadedUser> Users { get; } = users;

    public record LoadedRole(Guid Id, string Name);

    public record LoadedUser(string Name, Guid UserId, IEnumerable<Guid> AppRoleIds);
}
