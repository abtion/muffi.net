using FluentAssertions;
using MuffiNet.Backend.DomainModel;
using System;
using Xunit;

namespace MuffiNet.Backend.Tests.DomainModel;

[Collection("DomainModelTests")]
public class DomainModelBuilderTests {
    [Fact]
    public void Given_ServicesIsNull_When_CtorIsCalled_Then_ExceptionIsThrown() {
        Action act = () => {
            var x = new DomainModelBuilder(null);
        };

        act.Should().Throw<ArgumentNullException>();
    }
}