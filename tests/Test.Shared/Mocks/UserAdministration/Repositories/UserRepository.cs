using Domain.UserAdministration.Repositories;

namespace Test.Shared.Mocks.UserAdministration.Repositories;

public class UserRepository : IUpdateUserDetails, IGetUserAppRoleAssignments
{
    public Task<List<string?>> GetUserAppRoleAssignments(string userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task UpdateUser(string id, string DisplayName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
