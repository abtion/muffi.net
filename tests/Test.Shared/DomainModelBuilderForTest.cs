using DomainModel;
using DomainModel.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Test.Shared.TestData;

namespace Test.Shared;

public class DomainModelBuilderForTest {
    public void ConfigureServices(IServiceCollection services, string databaseName) {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase(databaseName));

        services.AddDomainModel();

        services.AddScoped<ExampleTestData>();
    }
}