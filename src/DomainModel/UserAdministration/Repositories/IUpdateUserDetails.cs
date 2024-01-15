namespace Domain.UserAdministration.Repositories;

public interface IUpdateUserDetails
{
    public Task UpdateUser(string id, string DisplayName, CancellationToken cancellationToken);
}
