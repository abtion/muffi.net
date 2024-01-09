using Domain.UserAdministration.Services;
using Microsoft.Graph.Models;
using System;
using System.Threading.Tasks;

namespace Test.Shared.Mocks.UserAdministration.Services;

public class UpdateUserInAzureIdentityMock : IUpdateUserInAzureIdentity
{
    public Task UpdateUser(User user)
    {
        throw new NotImplementedException();
    }
}
