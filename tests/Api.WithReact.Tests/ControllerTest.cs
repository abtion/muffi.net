using Api.WithReact.Hubs;
using DomainModel;
using DomainModel.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Test.Shared;
using Test.Shared.Mocks;
using Test.Shared.TestData;

namespace Api.WithReact.Tests;

public abstract class ControllerTest<TSystemUnderTest> : TestBase<TSystemUnderTest>
{
    protected override void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase(GetType().Name));

        services.AddDomainModel(configuration);
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