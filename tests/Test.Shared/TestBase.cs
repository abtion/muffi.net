using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

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
                var myConfiguration = new Dictionary<string, string>
                {
                    {"Key1", "Value1"},
                    {"Nested:Key1", "NestedValue1"},
                    {"Nested:Key2", "NestedValue2"}
                };

                var configuration = new ConfigurationBuilder()
                    .AddInMemoryCollection(myConfiguration!)
                    .Build();

                var serviceCollection = new ServiceCollection();
                AddServices(serviceCollection, configuration);

                serviceProvider = serviceCollection.BuildServiceProvider();
            }

            return serviceProvider!;
        }
    }

    protected virtual TSystemUnderTest GetSystemUnderTest()
    {
        return ServiceProvider.GetRequiredService<TSystemUnderTest>();
    }

    protected abstract void AddServices(IServiceCollection services, IConfiguration configuration);
}
