using DomainModel.Shared;

namespace DomainModel.UserAdministration.Queries;

public class LoadUserQuery : IQuery<LoadUserResponse>
{
    public LoadUserQuery(string userId)
    {
        UserId = userId;
    }

    public string UserId { get; init; }
}