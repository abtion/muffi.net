using Domain.UserAdministration.Repositories;

namespace Presentation.UserManagement.Repositories;

internal class AppRoleAssignmentRepository : IAssignAppRoleToUser, IRemoveAppRoleFromUser
{
    public Task AssignAppRoleToUser(string userId, string appRoleId)
    {
        throw new NotImplementedException();
    }

    public Task RemoveAppRoleFromUser(string appRoleAssignmentId)
    {
        throw new NotImplementedException();
    }
}
