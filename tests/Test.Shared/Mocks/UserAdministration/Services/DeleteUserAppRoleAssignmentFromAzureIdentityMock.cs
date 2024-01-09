using Domain.UserAdministration.Services;
using System;
using System.Threading.Tasks;

namespace Test.Shared.Mocks.UserAdministration.Services;

public class DeleteUserAppRoleAssignmentFromAzureIdentityMock : IDeleteUserAppRoleAssignmentFromAzureIdentity
{
    public Task DeleteAppRoleAssignment(string appRoleAssignmentId)
    {
        throw new NotImplementedException();
    }
}
