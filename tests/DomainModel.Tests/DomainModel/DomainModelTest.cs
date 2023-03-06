using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MuffiNet.Backend.Data;
using MuffiNet.Backend.DomainModel;
using MuffiNet.Backend.Models;
using MuffiNet.Test.Shared;
using System;
using System.Threading.Tasks;

namespace MuffiNet.Backend.Tests.DomainModel;

public abstract class DomainModelTest<T> {
    protected DomainModelTest() {
        var servicesBuilder = new DomainModelBuilderForTest();
        var serviceCollection = new ServiceCollection();

        servicesBuilder.ConfigureServices(serviceCollection, this.GetType().Name);

        ServiceProvider = serviceCollection.BuildServiceProvider();

        var context = ServiceProvider.GetService<ApplicationDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        Transaction = ServiceProvider.GetService<DomainModelTransaction>();
        UserManager = ServiceProvider.GetService<UserManager<ApplicationUser>>();

        // create a test user
        //UserManager.CreateAsync(ApplicationUserTestData.CreateApplicationUser());
    }

    protected internal IServiceProvider ServiceProvider { get; private set; }

    protected internal abstract Task<T> CreateSut();

    protected internal DomainModelTransaction Transaction { get; private set; }

    protected internal UserManager<ApplicationUser> UserManager { get; private set; }
}