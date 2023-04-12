using Api.WithReact.Hubs;
using DomainModel;
using DomainModel.Data;
using DomainModel.UserAdministration.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Test.Shared.Mocks;
using Test.Shared.Mocks.UserAdministration.Services;

namespace Api.WithReact.Tests;

public class ServiceConfigurationTests
{
    [Fact]
    public void CanResolveServiceDependencies()
    {
        var builder = WebApplication.CreateBuilder();

        // count the build-in services before adding application services
        var msServiceCount = builder.Services.Count;

        // add application services
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase(GetType().Name));

        var myConfiguration = new Dictionary<string, string>
                {
                    {"Key1", "Value1"},
                    {"Nested:Key1", "NestedValue1"},
                    {"Nested:Key2", "NestedValue2"}
                };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(myConfiguration!)
            .Build();

        builder.Services.AddDomainModel(configuration);
        builder.Services.AddApi();

        ReplaceServiceWithMock<IExampleHubContract, ExampleHubMock>(builder.Services);

        // replace the classes that communicate with Azure Identity with mocks that return test data
        ReplaceServiceWithMock<IConfiguredGraphServiceClient, ConfiguredGraphServiceClientMock>(builder.Services);
        ReplaceServiceWithMock<IGetAppRoleAssignmentsFromAzureIdentity, GetAppRoleAssignmentsFromAzureIdentityMock>(builder.Services);
        ReplaceServiceWithMock<IGetAppRolesFromAzureIdentity, GetAppRolesFromAzureIdentityMock>(builder.Services);
        ReplaceServiceWithMock<IGetUserAppRoleAssignmentsFromAzureIdentity, GetUserAppRoleAssignmentsFromAzureIdentityMock>(builder.Services);
        ReplaceServiceWithMock<IAddUserAppRoleAssignmentToAzureIdentity, AddUserAppRoleAssignmentToAzureIdentityMock>(builder.Services);
        ReplaceServiceWithMock<IDeleteUserAppRoleAssignmentFromAzureIdentity, DeleteUserAppRoleAssignmentFromAzureIdentityMock>(builder.Services);
        ReplaceServiceWithMock<IUpdateUserInAzureIdentity, UpdateUserInAzureIdentityMock>(builder.Services);
        ReplaceServiceWithMock<IGetUserFromAzureIdentity, GetUserFromAzureIdentityMock>(builder.Services);
        
        var app = builder.Build();

        var test = () =>
        {
            var customServices = builder.Services.Skip(msServiceCount).Where(p => !SkipList.Contains(p.ServiceType.FullName));

            foreach (var descriptor in customServices)
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine($"{descriptor.ServiceType}");
                    app.Services.GetService(descriptor.ServiceType);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(descriptor.ServiceType.FullName);
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
        };

        test.Should().NotThrow();
    }

    private void ReplaceServiceWithMock<TContract, TMockImplementation>(
        IServiceCollection services) 
        where TContract: class
        where TMockImplementation : class, TContract 
    {
        var serviceDescriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(TContract));

        services.Remove(serviceDescriptor);
        services.AddScoped<TContract, TMockImplementation>();
    }

    private IEnumerable<string> SkipList = new List<string>
    {
        "Microsoft.Extensions.Http.DefaultTypedHttpClientFactory`1+Cache[TClient]",
        "Microsoft.Extensions.Http.ITypedHttpClientFactory`1[TClient]",
        "MediatR.IPipelineBehavior`2",
    };
}
