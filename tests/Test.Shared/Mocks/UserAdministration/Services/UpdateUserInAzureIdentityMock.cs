using DomainModel.UserAdministration.Services;
using Microsoft.Graph;
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