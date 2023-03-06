using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DomainModel;
using DomainModel.Services.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace MuffiNet.FrontendReact.Tests.Authorization {
    public class UserRoleServiceTest {
        private static UserRoleService CreateUserRoleService() {
            var services = new ServiceCollection();

            var configMap = new Dictionary<string, string>()
            {
                { "ActiveDirectoryConfig:AppID", "16c880f4-e984-4ebe-8c4d-ae1a4a935a08" },
                { "ActiveDirectoryConfig:AppRegistrationID", "ff9d8859-4cde-408a-ba67-898200137521" },
                { "ActiveDirectoryConfig:AppClientID", "a507680f-bc92-4a58-aa69-917d9d33ea45" },
                //{ "ActiveDirectoryConfig:AppClientSecret", "___" }, // supplied by User Secret in dev, environment var (from Github Secrets) in CI
                { "ActiveDirectoryConfig:DirectoryTenantID", "63bdf5ff-bc58-43a9-8a61-1861f19f8e0e" },
                { "ActiveDirectoryConfig:BaseRoleID", "cc72dd23-ec9e-437b-a9b4-026f90558cae" },
                { "ActiveDirectoryConfig:AdminRoleID", "08fed2ce-47c6-42b9-97d6-7680e00e9677" },
                { "ActiveDirectoryConfig:AdminUserID", "9f1deff1-cf4d-4311-b1b2-0a6201e77361" }
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
        public async Task CanListAppRoles() {
            var service = CreateUserRoleService();

            var roles = await service.ListAppRoles();

            roles.Should().NotBeEmpty();
        }

        [Fact]
        public async Task CanListUsers() {
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
