using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MuffiNet.Backend.Data;
using MuffiNet.Backend.DomainModel;
using MuffiNet.Backend.Models;
using MuffiNet.Test.Shared.TestData;

namespace MuffiNet.Test.Shared;

public class DomainModelBuilderForTest {
    public void ConfigureServices(IServiceCollection services, string databaseName) {
        services.AddSingleton(Options.Create<OperationalStoreOptions>(new OperationalStoreOptions()));

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase(databaseName));

        // add an InMemory
        services.AddIdentityCore<ApplicationUser>()
            .AddUserManager<UserManager<ApplicationUser>>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddDomainModel();

        services.AddScoped<ExampleTestData>();
    }
}