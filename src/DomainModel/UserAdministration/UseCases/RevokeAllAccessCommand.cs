using Domain.Shared;

namespace Domain.UserAdministration.Commands;

public class RevokeAllAccessCommand : ICommand<RevokeAllAccessResponse>
{
    public RevokeAllAccessCommand(string userId)
    {
        UserId = userId;
    }

    public string UserId { get; init; }
}