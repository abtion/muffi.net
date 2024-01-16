namespace Domain.UserAdministration.Entities;

public record AppRoleEntity(Guid? Id, string? DisplayName)
{
    public Guid? Id { get; } = Id;

    public string? DisplayName { get; } = DisplayName;
}
