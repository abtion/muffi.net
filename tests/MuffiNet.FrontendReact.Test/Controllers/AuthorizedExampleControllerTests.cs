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
using MuffiNet.Backend.DomainModel.Commands.ExampleCreateCommand;

namespace MuffiNet.FrontendReact.Test.Controllers
{
    public class AuthorizedExampleControllerTests : ControllerTest
    {

        [Fact]
        public async Task Given_RequestIsValid_When_ExampleQueryIsCalled_Then_ReturnTypeIsCorrect()
        {
            // Arrange
            var transaction = ServiceProvider.GetService<DomainModelTransaction>();
            transaction.ResetExampleEntities();

            var testData = new ExampleTestData(transaction);
            await testData.AddExampleEntitiesToDatabase(5);

            var controller = new AuthorizedExampleController();
            var handler = new ExampleQueryHandler(transaction);

            int exampleEntityId = 3;

            // Act
            var response = await controller.ExampleQuery(handler, exampleEntityId, new CancellationToken());

            // Assert
            response.Value.Should().BeOfType<ExampleQueryResponse>();
        }

        [Fact]
        public async Task Given_RequestIsValid_When_ExampleCreateCommandIsCalled_Then_ReturnTypeIsCorrect()
        {
            // Arrange
            var transaction = ServiceProvider.GetService<DomainModelTransaction>();
            transaction.ResetExampleEntities();

            var exampleHubMock = new ExampleHubMock();

            var controller = new AuthorizedExampleController();
            var handler = new ExampleCreateCommandHandler(transaction, exampleHubMock);

            var request = new ExampleCreateCommandRequest()
            {
                Name = "Integration",
                Description = "Test",
                Email = "integration@test.net",
                Phone = "+45 70 70 70 70"
            };

            var cancellationToken = new CancellationToken();

            // Act
            var response = await controller.ExampleCreateCommand(handler, request, cancellationToken);

            // Assert
            response.Value.Should().BeOfType<ExampleCreateCommandResponse>();
        }
    }
}