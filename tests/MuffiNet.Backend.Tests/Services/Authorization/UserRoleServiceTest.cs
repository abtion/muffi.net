using MuffiNet.Backend.Services.Authorization;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MuffiNet.Backend.DomainModel;
using System.Collections.Generic;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace MuffiNet.FrontendReact.Tests.Authorization
{
    public class UserRoleServiceTest
    {
        private static UserRoleService CreateUserRoleService()
        {
            var services = new ServiceCollection();

            var configMap = new Dictionary<string, string>()
            {
                { "ActiveDirectoryConfig:AppID", "16c880f4-e984-4ebe-8c4d-ae1a4a935a08" },
                { "ActiveDirectoryConfig:AppRegistrationID", "ff9d8859-4cde-408a-ba67-898200137521" },
                { "ActiveDirectoryConfig:AppClientID", "a507680f-bc92-4a58-aa69-917d9d33ea45" },
                // { "ActiveDirectoryConfig:AppClientSecret", "___" }, // supplied by User Secret in dev, environment var (from Github Secrets) in CI
                { "ActiveDirectoryConfig:DirectoryTenantID", "63bdf5ff-bc58-43a9-8a61-1861f19f8e0e" },
                { "ActiveDirectoryConfig:AdminRoleID", "d2a3a58e-9abf-4cd3-a2a5-7a60fe1e9f47" },
                { "ActiveDirectoryConfig:AdminUserID", "9f1deff1-cf4d-4311-b1b2-0a6201e77361" },
            };

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(configMap)                     // built-in test defaults
                .AddUserSecrets(typeof(UserRoleServiceTest).Assembly) // for running tests locally
                .AddEnvironmentVariables()                            // for running tests in CI
                .Build();

            services.AddUserRoleService(config);

            var provider = services.BuildServiceProvider();

            return provider.GetService<UserRoleService>();
        }

        [Fact]
        public async Task CanListAppRoles()
        {
            var service = CreateUserRoleService();

            var roles = await service.ListAppRoles();

            roles.Should().NotBeEmpty();
        }

        [Fact]
        public async Task CanListUsers()
        {
            var service = CreateUserRoleService();

            var users = await service.ListUsers();

            users.Should().NotBeEmpty();
        }

        // TODO add a dummy User to AD for integration testing?
        //[Fact]
        //public async Task CanGetUserDetails()
        //{
        //    var service = CreateUserRoleService();

        //    var details = await service.GetUserDetails(".......");

        //    details.Should().NotBeNull();
        //}
    }
}
