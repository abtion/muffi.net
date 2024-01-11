using Domain.UserAdministration.Services;
using Microsoft.Graph.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Shared.Mocks.UserAdministration.Services;

public class GetAppRoleAssignmentsFromAzureIdentityMock : IGetAppRoleAssignmentsFromAzureIdentity
{
    public Task<IQueryable<AppRoleAssignment>> GetAppRoleAssignmentsForUser(string userId)
    {
        throw new NotImplementedException();
    }

    Task<IQueryable<AppRoleAssignment>> IGetAppRoleAssignmentsFromAzureIdentity.GetAppRoleAssignments()
    {
        throw new NotImplementedException();
    }
}
