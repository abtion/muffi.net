namespace Domain.UserAdministration.Repositories;

public interface IAssignAppRoleToUser
{
    public Task AssignAppRoleToUser(string userId, string appRoleId, CancellationToken cancellationToken);
}
