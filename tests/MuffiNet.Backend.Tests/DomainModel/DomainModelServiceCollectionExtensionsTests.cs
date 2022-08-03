using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MuffiNet.Backend.DomainModel;
using MuffiNet.Backend.Services.Authorization;
using System;
using System.Collections.Generic;
using Xunit;

namespace MuffiNet.Backend.Tests.DomainModel;

[Collection("DomainModelTests")]
public class DomainModelServiceCollectionExtensionsTests
{
    [Fact]
    public void Given_True_When_AddDomainModelIsCalled_Then_NoExceptionIsThrown()
    {
        var services = new ServiceCollection();

        Action act = () => services.AddDomainModel();

        act.Should().NotThrow();
    }

    [Fact]
    public void CanRegisterUserRoleServiceDependencies()
    {
        var services = new ServiceCollection();

        var configMap = new Dictionary<string, string>()
        {
            { "ActiveDirectoryConfig:AppID", "16c880f4-e984-4ebe-8c4d-ae1a4a935a08" },
            { "ActiveDirectoryConfig:AppRegistrationID", "ff9d8859-4cde-408a-ba67-898200137521" },
            { "ActiveDirectoryConfig:AppClientID", "a507680f-bc92-4a58-aa69-917d9d33ea45" },
            { "ActiveDirectoryConfig:AppClientSecret", "AVu8Q~XdNmhJwWGO6ACbbaflT~fbGaO4zpF77ddc" },
            { "ActiveDirectoryConfig:DirectoryTenantID", "63bdf5ff-bc58-43a9-8a61-1861f19f8e0e" },
            { "ActiveDirectoryConfig:AdminRoleID", "d2a3a58e-9abf-4cd3-a2a5-7a60fe1e9f47" },
            { "ActiveDirectoryConfig:AdminUserID", "9f1deff1-cf4d-4311-b1b2-0a6201e77361" },
        };

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(configMap)
            .Build();

        services.AddUserRoleService(config);

        var provider = services.BuildServiceProvider();

        provider.GetService<UserRoleService>().Should().BeOfType<UserRoleService>("the configuration instance is correctly registered and resolved");
    }
}
