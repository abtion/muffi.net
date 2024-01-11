using Domain.UserAdministration.Entities;

namespace Domain.UserAdministration.Repositories;

public interface IGetUserAppRoleAssignments
{
    public Task<IQueryable<UserWithAppRoleAssignmentEntity>> GetUserAppRoleAssignments(string userId);
}
