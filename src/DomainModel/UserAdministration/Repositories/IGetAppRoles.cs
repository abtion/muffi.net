using Domain.UserAdministration.Entities;

namespace Domain.UserAdministration.Repositories;

public interface IGetAppRoles
{
    public Task<List<AppRoleEntity>> GetAppRoles();
}
