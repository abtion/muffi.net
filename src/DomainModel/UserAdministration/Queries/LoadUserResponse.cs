namespace DomainModel.UserAdministration.Queries;

public class LoadUserResponse
{
    public string Email { get; init; }

    public LoadUserResponse(string email)
    {
        Email = email;
    }
}