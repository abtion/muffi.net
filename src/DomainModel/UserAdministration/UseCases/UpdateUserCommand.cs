using Domain.Shared;
using System;
using System.Collections.Generic;

namespace Domain.UserAdministration.Commands;

public class UpdateUserCommand : ICommand<UpdateUserResponse>
{
    public UpdateUserCommand(string name, Guid userId, IEnumerable<Guid> appRoleIds)
    {
        Name = name;
        UserId = userId;
        AppRoleIds = appRoleIds;
    }

    public string Name { get; init; }

    public Guid UserId { get; init; }

    public IEnumerable<Guid> AppRoleIds { get; init; }
}
