using Domain.Shared;

namespace Presentation.UserAdministration.Dtos;

public class UpdateUserCommand(string name, Guid userId, IEnumerable<Guid> appRoleIds) : ICommand<UpdateUserResponse>
{
    public string Name { get; init; } = name;

    public Guid UserId { get; init; } = userId;

    public IEnumerable<Guid> AppRoleIds { get; init; } = appRoleIds;
}
