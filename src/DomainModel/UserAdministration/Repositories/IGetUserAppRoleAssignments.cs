using Domain.UserAdministration.Entities;

namespace Domain.UserAdministration.Repositories;

public interface IGetUserAppRoleAssignments
{
    public Task<List<string?>> GetUserAppRoleAssignments(string userId, CancellationToken cancellationToken);
}
