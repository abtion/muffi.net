namespace Domain.UserAdministration.Entities;

public record UserEntity(string UserId, string Email)
{
    public string UserId { get; } = UserId;

    public string Email { get;  } = Email;
}
