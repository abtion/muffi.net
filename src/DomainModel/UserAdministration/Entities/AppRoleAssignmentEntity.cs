namespace Domain.UserAdministration.Entities;

public record AppRoleAssignmentEntity(string? Id, Guid? AppRoleId, Guid? PrincipalId, string? PrincipalDisplayName)
{
    public string? Id { get; } = Id;

    public Guid? AppRoleId { get; } = AppRoleId;

    public Guid? PrincipalId { get; } = PrincipalId;

    public string? PrincipalDisplayName { get; } = PrincipalDisplayName;
}
