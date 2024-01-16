using Domain.Shared;

namespace Presentation.UserAdministration.Dtos;

public class LoadUserQuery(string userId) : IQuery<LoadUserResponse>
{
    public string UserId { get; init; } = userId;
}