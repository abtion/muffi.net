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
                var serviceCollection = new ServiceCollection();

                AddServices(serviceCollection);

                serviceProvider = serviceCollection.BuildServiceProvider();
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
