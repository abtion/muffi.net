using Domain.Shared;
using Domain.UserAdministration.Services;
using Presentation.UserAdministration.Dtos;

namespace Presentation.UserAdministration.Queries;

public class LoadUserQueryHandler(IGetUserFromAzureIdentity GetUserFromAzureIdentity) : IQueryHandler<LoadUserQuery, LoadUserResponse>
{
    public async Task<LoadUserResponse> Handle(LoadUserQuery request, CancellationToken cancellationToken)
    {
        var user = await GetUserFromAzureIdentity.GetUser(request.UserId);

        if (user.Mail is not null)
            return new LoadUserResponse(user.Mail);

        return new LoadUserResponse(string.Empty);
    }
}
