using DomainModel.Shared;
using DomainModel.UserAdministration.Services;
using System.Threading;
using System.Threading.Tasks;

namespace DomainModel.UserAdministration.Queries;

public class LoadUserQueryHandler : IQueryHandler<LoadUserQuery, LoadUserResponse>
{
    private readonly IGetUserFromAzureIdentity getUserFromAzureIdentity;

    public LoadUserQueryHandler(IGetUserFromAzureIdentity getUserFromAzureIdentity)
    {
        this.getUserFromAzureIdentity = getUserFromAzureIdentity;
    }

    public async Task<LoadUserResponse> Handle(LoadUserQuery request, CancellationToken cancellationToken)
    {
        var user = await getUserFromAzureIdentity.GetUser(request.UserId);

        return new LoadUserResponse(user.Mail);
    }
}