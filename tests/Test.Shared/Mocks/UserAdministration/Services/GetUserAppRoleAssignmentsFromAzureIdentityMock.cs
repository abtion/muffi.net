using DomainModel.UserAdministration.Services;
using Microsoft.Graph;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Shared.Mocks.UserAdministration.Services;

public class GetUserAppRoleAssignmentsFromAzureIdentityMock : IGetUserAppRoleAssignmentsFromAzureIdentity
{
    public Task<IQueryable<AppRoleAssignment>> GetUserAppRoleAssignments(string userId)
    {
        throw new NotImplementedException();
    }
}