using Domain.UserAdministration.Entities;
using Domain.UserAdministration.Repositories;

namespace Test.Shared.Mocks.UserManagement.Repositories;

public class AppRoleAssignmentRepository() : IAssignAppRoleToUser, IRemoveAppRoleFromUser, IGetAppRoleAssignmentForUser, IGetAppRoleAssigment
{
    public async Task AssignAppRoleToUser(string userId, string appRoleId, CancellationToken cancellationToken)
    {
        AssignAppRoleToUserCounter++;

        await Task.CompletedTask;
    }

    public int AssignAppRoleToUserCounter { get; set; }

    public async Task RemoveAppRoleFromUser(string appRoleAssignmentId, CancellationToken cancellationToken)
    {
        RemoveAppRoleFromUserCounter++;

        await Task.CompletedTask;
    }

    public Task<List<AppRoleAssignmentEntity>> GetAppRoleAssignments()
    {
        throw new NotImplementedException();
    }

    public Task<List<AppRoleAssignmentEntity>> GetAppRoleAssignmentsForUser(string userId)
    {
        throw new NotImplementedException();
    }

    public int RemoveAppRoleFromUserCounter { get; set; }
}
