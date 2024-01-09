using Domain.UserAdministration.Services;
using Microsoft.Graph.Models;
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
