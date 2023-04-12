﻿using DomainModel.UserAdministration.Services;
using Microsoft.Graph;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Shared.Mocks.UserAdministration.Services;

public class GetAppRolesFromAzureIdentityMock : IGetAppRolesFromAzureIdentity
{
    public Task<IQueryable<AppRole>> GetAppRoles()
    {
        throw new NotImplementedException();
    }
}