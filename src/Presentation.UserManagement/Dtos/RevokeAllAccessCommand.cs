using Domain.Shared;

namespace Presentation.UserAdministration.Dtos;

public class RevokeAllAccessCommand(string userId) : ICommand<RevokeAllAccessResponse>
{
    public string UserId { get; init; } = userId;
}