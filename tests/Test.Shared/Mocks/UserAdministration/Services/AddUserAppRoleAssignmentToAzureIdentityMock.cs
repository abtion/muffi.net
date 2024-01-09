using Domain.UserAdministration.Services;
using System;
using System.Threading.Tasks;

namespace Test.Shared.Mocks.UserAdministration.Services
{
    public class AddUserAppRoleAssignmentToAzureIdentityMock : IAddUserAppRoleAssignmentToAzureIdentity
    {
        public Task AddUserAppRoleAssignment(string userId, string appRoleId)
        {
            throw new NotImplementedException();
        }
    }
}
