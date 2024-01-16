using Domain.UserAdministration.Entities;

namespace Domain.UserAdministration.Repositories;

public interface IGetAllAppRoleAssignments
{
    public Task<IQueryable<UserWithAppRoleAssignmentEntity>> GetUsersWithAppRoleAssignments();
}
