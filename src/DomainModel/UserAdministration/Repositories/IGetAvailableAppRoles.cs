using Domain.UserAdministration.Entities;

namespace Domain.UserAdministration.Repositories;

public interface IGetAvailableAppRoles
{
    public Task<IQueryable<AppRoleEntity>> GetAppRoles();
}