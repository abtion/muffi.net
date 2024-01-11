namespace Domain.UserAdministration.Entities;

public record AppRoleEntity(string Id, string DisplayName)
{
    public string Id { get; } = Id;

    public string DisplayName { get; } = DisplayName;
}
