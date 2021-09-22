using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MuffiNet.Backend.DomainModel;
using MuffiNet.Backend.DomainModel.Commands.ExampleCreateCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MuffiNet.Backend.Tests.DomainModel
{
    [Collection("DomainModelTests")]
    public class DomainModelServiceCollectionExtensionsTests
    {
        [Fact]
        public void Given_True_When_AddDomainModelIsCalled_Then_NoExceptionIsThrown()
        {
            var services = new ServiceCollection();

            Action act = () => services.AddDomainModel();

            act.Should().NotThrow();
        }
    }
}