using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using DomainModel.Data;
using DomainModel;
using DomainModel.Models;
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