using Domain.UserAdministration.Entities;
using Domain.UserAdministration.Repositories;

namespace Test.Shared.Mocks.UserAdministration.Repositories;

public class AppRoleRepository : IGetAppRoles
{
    public Task<List<AppRoleEntity>> GetAppRoles()
    {
        throw new NotImplementedException();
    }
}
