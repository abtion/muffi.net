using Domain.UserAdministration.Entities;

namespace Domain.UserAdministration.Repositories;

public interface IGetUserDetails
{
    public Task<UserEntity> GetUser(string userId);
}
