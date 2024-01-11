namespace Domain.UserAdministration.Entities;

public record UserWithAppRoleAssignmentEntity(string DisplayName, Guid UserId, IEnumerable<Guid> AppRoleIds)
{
    public Guid UserId { get; } = UserId;

    public string DisplayName { get; } = DisplayName;

    public IEnumerable<Guid> AppRoleIds { get; } = AppRoleIds;
}
