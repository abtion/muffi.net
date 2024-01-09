using Domain.Shared;
using Domain.UserAdministration.Services;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.UserAdministration.Queries;

public class LoadUserQueryHandler : IQueryHandler<LoadUserQuery, LoadUserResponse>
{
    private readonly IGetUserFromAzureIdentity getUserFromAzureIdentity;

    public LoadUserQueryHandler(IGetUserFromAzureIdentity getUserFromAzureIdentity)
    {
        this.getUserFromAzureIdentity = getUserFromAzureIdentity;
    }

    public async Task<LoadUserResponse> Handle(
        LoadUserQuery request,
        CancellationToken cancellationToken
    )
    {
        var user = await getUserFromAzureIdentity.GetUser(request.UserId);

        if (user.Mail is not null)
            return new LoadUserResponse(user.Mail);

        return new LoadUserResponse(string.Empty);
    }
}
