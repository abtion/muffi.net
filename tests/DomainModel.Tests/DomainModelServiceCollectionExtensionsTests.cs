using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace DomainModel.Tests;

[Collection("DomainModelTests")]
public class DomainModelServiceCollectionExtensionsTests {
    [Fact]
    public void Given_True_When_AddDomainModelIsCalled_Then_NoExceptionIsThrown() {
        var services = new ServiceCollection();

        Action act = () => services.AddDomainModel();

        act.Should().NotThrow();
    }
}
