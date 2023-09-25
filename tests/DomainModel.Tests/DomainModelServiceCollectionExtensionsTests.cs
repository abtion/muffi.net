using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace DomainModel.Tests;

[Collection("DomainModelTests")]
public class DomainModelServiceCollectionExtensionsTests
{
    [Fact]
    public void Given_True_When_AddDomainModelIsCalled_Then_NoExceptionIsThrown()
    {
        var myConfiguration = new Dictionary<string, string>
        {
            { "Key1", "Value1" },
            { "Nested:Key1", "NestedValue1" },
            { "Nested:Key2", "NestedValue2" }
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(myConfiguration)
            .Build();

        var services = new ServiceCollection();

        Action act = () => services.AddDomainModel(configuration);

        act.Should().NotThrow();
    }
}
