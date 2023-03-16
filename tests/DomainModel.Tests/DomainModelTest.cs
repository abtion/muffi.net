using Api.WithReact.Hubs;
using DomainModel.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Test.Shared;
using Test.Shared.Mocks;
using Test.Shared.TestData;

namespace DomainModel.Tests;

public abstract class DomainModelTest<TSystemUnderTest> : TestBase<TSystemUnderTest>
{
    protected override void AddServices(IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase(GetType().Name));

        services.AddDomainModel();
        //services.AddScoped<IExampleHubContract, ExampleHubMock>();

        services.AddScoped<ExampleTestData>();
    }
}