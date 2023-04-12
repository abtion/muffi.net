using DomainModel.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Test.Shared;
using Test.Shared.TestData;

namespace DomainModel.Tests;

public abstract class DomainModelTest<TSystemUnderTest> : TestBase<TSystemUnderTest>
{
    protected override void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase(GetType().Name));

        //var context = services.GetRequiredService<ApplicationDbContext>();
        //context.Database.EnsureDeleted();
        //context.Database.EnsureCreated();

        services.AddDomainModel(configuration);
        //services.AddScoped<IExampleHubContract, ExampleHubMock>();

        services.AddScoped<ExampleTestData>();
    }
}