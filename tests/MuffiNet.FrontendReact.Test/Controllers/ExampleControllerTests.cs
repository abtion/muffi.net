using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MuffiNet.Backend.DomainModel;
using MuffiNet.Backend.DomainModel.Commands.ExampleCreateCommand;
using MuffiNet.Backend.DomainModel.Commands.ExampleDeleteCommand;
using MuffiNet.Backend.DomainModel.Queries.ExampleQuery;
using MuffiNet.Backend.DomainModel.Queries.ExampleQueryAll;
using MuffiNet.Backend.Services;
using MuffiNet.FrontendReact.Controllers;
using MuffiNet.Test.Shared.Mocks;
using MuffiNet.Test.Shared.TestData;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MuffiNet.FrontendReact.Test.Controllers;

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
        var handler = new ExampleQueryHandler(transaction, new ExampleReverseStringService());

        int exampleEntityId = 3;

        // Act
        var response = await controller.ExampleQuery(handler, exampleEntityId, new CancellationToken());

        // Assert
        response.Value.Should().BeOfType<ExampleQueryResponse>();
    }

    [Fact]
    public async Task Given_RequestIsValid_When_ExampleQueryAllIsCalled_Then_ReturnTypeIsCorrect()
    {
        // Arrange
        var transaction = ServiceProvider.GetService<DomainModelTransaction>();
        transaction.ResetExampleEntities();

        var testData = new ExampleTestData(transaction);
        await testData.AddExampleEntitiesToDatabase(5);

        var controller = new ExampleController();
        var handler = new ExampleQueryAllHandler(transaction);

        // Act
        var response = await controller.ExampleQueryAll(handler, new CancellationToken());

        // Assert
        response.Value.Should().BeOfType<ExampleQueryAllResponse>();
    }

    [Fact]
    public async Task Given_RequestIsValid_When_ExampleCreateCommandIsCalled_Then_ReturnTypeIsCorrect()
    {
        // Arrange
        var transaction = ServiceProvider.GetService<DomainModelTransaction>();
        transaction.ResetExampleEntities();

        var exampleHubMock = new ExampleHubMock();

        var controller = new ExampleController();
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

    [Fact]
    public async Task Given_RequestIsValid_When_ExampleDeleteCommandIsCalled_Then_ReturnTypeIsCorrect()
    {
        // Arrange
        var transaction = ServiceProvider.GetService<DomainModelTransaction>();
        transaction.ResetExampleEntities();

        var testData = new ExampleTestData(transaction);
        await testData.AddExampleEntitiesToDatabase(5);

        var controller = new ExampleController();
        var handler = new ExampleDeleteCommandHandler(transaction, new ExampleHubMock());

        var request = new ExampleDeleteCommandRequest()
        {
            Id = 3
        };

        // Act
        var response = await controller.ExampleDeleteCommand(handler, request, new CancellationToken());

        // Assert
        response.Value.Should().BeOfType<ExampleDeleteCommandResponse>();
    }
}