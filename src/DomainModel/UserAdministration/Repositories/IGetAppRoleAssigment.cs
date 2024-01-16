using Domain.UserAdministration.Entities;

namespace Domain.UserAdministration.Repositories;

public interface IGetAppRoleAssigment
{
    public Task<List<AppRoleAssignmentEntity>> GetAppRoleAssignments();
}
