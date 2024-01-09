using Infrastructure;
using Microsoft.Extensions.Configuration;
using Presentation;
using Test.Shared;
using Test.Shared.TestData;

namespace Domain.Tests;

public abstract class DomainModelTest<TSystemUnderTest> : TestBase<TSystemUnderTest>
{
    protected override void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDomain();
        services.AddInfrastructure(configuration, true);
        services.AddPresentation();

        services.AddScoped<ExampleTestData>();
    }
}