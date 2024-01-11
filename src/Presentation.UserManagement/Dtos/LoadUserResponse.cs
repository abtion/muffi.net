namespace Presentation.UserAdministration.Dtos;

public class LoadUserResponse(string email)
{
    public string Email { get; init; } = email;
}