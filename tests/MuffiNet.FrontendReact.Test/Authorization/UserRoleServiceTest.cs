using MuffiNet.FrontendReact.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace MuffiNet.FrontendReact.Tests.Authorization
{
    public class UserRoleServiceTest
    {
        private ActiveDirectoryConfig credentials => new ActiveDirectoryConfig()
        {
            AppID = "16c880f4-e984-4ebe-8c4d-ae1a4a935a08",
            AppRegistrationID = "ff9d8859-4cde-408a-ba67-898200137521",
            AppClientID = "a507680f-bc92-4a58-aa69-917d9d33ea45",
            AppClientSecret = "AVu8Q~XdNmhJwWGO6ACbbaflT~fbGaO4zpF77ddc",
            DirectoryTenantID = "63bdf5ff-bc58-43a9-8a61-1861f19f8e0e",
            AdminRoleID = "d2a3a58e-9abf-4cd3-a2a5-7a60fe1e9f47",
            AdminUserID = "9f1deff1-cf4d-4311-b1b2-0a6201e77361",
        };

        [Fact]
        public async Task CanListAppRoles()
        {
            var service = new UserRoleService(credentials);

            var roles = await service.ListAppRoles();

            roles.Should().NotBeEmpty();
        }


    }
}
