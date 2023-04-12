using DomainModel.UserAdministration.Services;
using Microsoft.Graph;
using System;
using System.Threading.Tasks;

namespace Test.Shared.Mocks.UserAdministration.Services;

public class GetUserFromAzureIdentityMock : IGetUserFromAzureIdentity
{
    public Task<User> GetUser(string userId)
    {
        throw new NotImplementedException();
    }
}