using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using MuffiNet.FrontendReact.Controllers;
using MuffiNet.Backend.DomainModel;
using MuffiNet.Backend.Models;
using MuffiNet.Backend.Services;
using MuffiNet.Test.Shared.Mocks;
using MuffiNet.Test.Shared.TestData;
using Xunit;
using MuffiNet.Backend.DomainModel.Queries.ExampleQuery;
using FluentAssertions;
using MuffiNet.Backend.DomainModel.Commands.ExampleCommand;

namespace MuffiNet.FrontendReact.Test.Controllers
{
    [Collection("Controller")]
    public class ExampleControllerTests : ControllerTest
    {

        [Fact]
        public async Task Given_RequestIsValid_When_ExampleQueryIsCalled_Then_ReturnTypeIsCorrect()
        {
            // Arrange
            var transaction = ServiceProvider.GetService<DomainModelTransaction>();
            transaction.ResetExampleEntities();

            var testData = new ExampleTestData(transaction);
            await testData.AddExampleEntitiesToDatabase(5);

            var controller = new ExampleController();
            var handler = new ExampleQueryHandler(transaction);

            int exampleEntityId = 3;

            // Act
            var response = await controller.ExampleQuery(handler, exampleEntityId, new CancellationToken());

            // Assert
            response.Value.Should().BeOfType<ExampleQueryResponse>();
        }

        [Fact]
        public async Task Given_RequestIsValid_When_ExampleCommandIsCalled_Then_ReturnTypeIsCorrect()
        {
            // Arrange
            var transaction = ServiceProvider.GetService<DomainModelTransaction>();
            transaction.ResetExampleEntities();

            var exampleHubMock = new ExampleHubMock();

            var controller = new ExampleController();
            var handler = new ExampleCommandHandler(transaction, exampleHubMock);

            var request = new ExampleCommandRequest()
            {
                Name = "Integration",
                Description = "Test",
                Email = "integration@test.net",
                Phone = "+45 70 70 70 70"
            };

            var cancellationToken = new CancellationToken();

            // Act
            var response = await controller.ExampleCommand(handler, request, cancellationToken);

            // Assert
            response.Value.Should().BeOfType<ExampleCommandResponse>();
        }
    }
}