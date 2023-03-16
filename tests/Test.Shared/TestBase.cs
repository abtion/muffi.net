using DomainModel.Data;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Test.Shared;

public abstract class TestBase<TSystemUnderTest> where TSystemUnderTest : notnull
{
    private IServiceProvider? serviceProvider;

    protected IServiceProvider ServiceProvider
    {
        get
        {
            if (serviceProvider == null)
            {

                var servicesBuilder = new DomainModelBuilderForTest();
                var serviceCollection = new ServiceCollection();

                AddServices(serviceCollection);

                servicesBuilder.ConfigureServices(serviceCollection, GetType().Name);

                serviceProvider = serviceCollection.BuildServiceProvider();

                var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            return serviceProvider!;
        }
    }

    protected virtual TSystemUnderTest GetSystemUnderTest()
    {
        return ServiceProvider.GetRequiredService<TSystemUnderTest>();
    }

    protected abstract void AddServices(IServiceCollection services);
}
