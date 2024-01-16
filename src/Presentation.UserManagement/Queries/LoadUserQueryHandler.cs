using Domain.Shared;
using Domain.UserAdministration.Repositories;
using Presentation.UserAdministration.Dtos;

namespace Presentation.UserAdministration.Queries;

public class LoadUserQueryHandler(IGetUserDetails getUserDetails) : IQueryHandler<LoadUserQuery, LoadUserResponse>
{
    public async Task<LoadUserResponse> Handle(LoadUserQuery request, CancellationToken cancellationToken)
    {
        var user = await getUserDetails.GetUser(request.UserId);

        return user.Email is not null ? new LoadUserResponse(user.Email) : new LoadUserResponse(string.Empty);
    }
}
