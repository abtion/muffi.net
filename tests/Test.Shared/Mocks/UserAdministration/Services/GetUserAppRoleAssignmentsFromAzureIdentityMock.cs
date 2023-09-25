using DomainModel.UserAdministration.Services;
using Microsoft.Graph.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Shared.Mocks.UserAdministration.Services;

public class GetUserAppRoleAssignmentsFromAzureIdentityMock
    : IGetUserAppRoleAssignmentsFromAzureIdentity
{
    public Task<IQueryable<AppRoleAssignment>> GetUserAppRoleAssignments(string userId)
    {
        throw new NotImplementedException();
    }
}
