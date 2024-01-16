using Domain.UserAdministration.Entities;

namespace Domain.UserAdministration.Repositories;

public interface IGetAppRoleAssignmentForUser
{
    public Task<List<AppRoleAssignmentEntity>> GetAppRoleAssignmentsForUser(string userId, CancellationToken cancellationToken);
}
