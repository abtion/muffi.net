using DomainModel.Data;
using DomainModel.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Test.Shared;
using System;
using System.Threading.Tasks;

namespace DomainModel.Tests;

public abstract class DomainModelTest<T>
{
    protected DomainModelTest()
    {
        var servicesBuilder = new DomainModelBuilderForTest();
        var serviceCollection = new ServiceCollection();

        servicesBuilder.ConfigureServices(serviceCollection, GetType().Name);

        ServiceProvider = serviceCollection.BuildServiceProvider();

        var context = ServiceProvider.GetService<ApplicationDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        Transaction = ServiceProvider.GetService<DomainModelTransaction>();
        //UserManager = ServiceProvider.GetService<UserManager<ApplicationUser>>();

        // create a test user
        //UserManager.CreateAsync(ApplicationUserTestData.CreateApplicationUser());
    }

    protected internal IServiceProvider ServiceProvider { get; private set; }

    protected internal abstract Task<T> CreateSut();

    protected internal DomainModelTransaction Transaction { get; private set; }

    //protected internal UserManager<ApplicationUser> UserManager { get; private set; }
}