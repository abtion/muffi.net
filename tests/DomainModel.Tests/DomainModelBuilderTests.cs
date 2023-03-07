using FluentAssertions;
using System;
using Xunit;

namespace DomainModel.Tests;

[Collection("DomainModelTests")]
public class DomainModelBuilderTests
{
    [Fact]
    public void Given_ServicesIsNull_When_CtorIsCalled_Then_ExceptionIsThrown()
    {
        Action act = () =>
        {
            var x = new DomainModelBuilder(null);
        };

        act.Should().Throw<ArgumentNullException>();
    }
}