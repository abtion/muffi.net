namespace Domain.UserAdministration.Repositories;

public interface IRemoveAppRoleFromUser
{
    public Task RemoveAppRoleFromUser(string appRoleAssignmentId, CancellationToken cancellationToken);
}
