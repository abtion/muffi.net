using Api.WithReact.Hubs;
using Domain;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Presentation;
using Test.Shared;
using Test.Shared.Mocks;
using Test.Shared.TestData;

namespace Api.WithReact.Tests;

public abstract class ControllerTest<TSystemUnderTest> : TestBase<TSystemUnderTest>
{
    protected override void AddServices(IServiceCollection services, IConfiguration configuration)
    {

        services.AddDomain();
        services.AddInfrastructure(configuration, true);
        services.AddPresentation();
        services.AddApi();

        //// replace ExampleHub with mock implementation
        //var serviceDescriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(IExampleHubContract));
        //services.Remove(serviceDescriptor);
        services.AddScoped<IExampleHubContract, ExampleHubMock>();

        services.AddScoped<ExampleTestData>();
    }
    protected internal ExampleTestData TestData
    {
        get
        {
            return ServiceProvider.GetRequiredService<ExampleTestData>();
        }
    }
}