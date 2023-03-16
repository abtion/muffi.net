using DomainModel;
using DomainModel.Example.Commands;
using DomainModel.Example.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using Test.Shared.TestData;

namespace Api.WithReact.Tests.Controllers;

[Collection("Controller")]
public class ExampleControllerTests : ControllerTest<ExampleController>
{
    public ExampleControllerTests()
    {
        transaction = ServiceProvider.GetRequiredService<DomainModelTransaction>();
        transaction.ResetExampleEntities();

        testData = ServiceProvider.GetRequiredService<ExampleTestData>();
    }

    private readonly DomainModelTransaction transaction;
    private readonly ExampleTestData testData;

    protected override ExampleController GetSystemUnderTest()
    {
        return new ExampleController();
    }

    [Fact]
    public async Task Given_RequestIsValid_When_ExampleQueryIsCalled_Then_ReturnTypeIsCorrect()
    {
        // Arrange
        await testData.AddExampleEntitiesToDatabase(5);

        var controller = GetSystemUnderTest();
        var handler = ServiceProvider.GetRequiredService<ExampleLoadSingleQueryHandler>();

        int exampleEntityId = 3;

        // Act
        var response = await controller.ExampleQuery(handler, exampleEntityId, new CancellationToken());

        // Assert
        response.Should().BeOfType<ExampleLoadSingleResponse>();
    }

    [Fact]
    public async Task Given_RequestIsValid_When_ExampleQueryAllIsCalled_Then_ReturnTypeIsCorrect()
    {
        // Arrange
        await testData.AddExampleEntitiesToDatabase(5);

        var controller = GetSystemUnderTest();
        var handler = ServiceProvider.GetRequiredService<ExampleLoadAllQueryHandler>();

        // Act
        var response = await controller.ExampleQueryAll(handler, new CancellationToken());

        // Assert
        response.Should().BeOfType<ExampleLoadAllResponse>();
    }

    [Fact]
    public async Task Given_RequestIsValid_When_ExampleCreateCommandIsCalled_Then_ReturnTypeIsCorrect()
    {
        // Arrange
        var mediator = ServiceProvider.GetService<IMediator>();

        var controller = GetSystemUnderTest();
        var handler = ServiceProvider.GetRequiredService<ExampleCreateCommandHandler>();

        var request = new ExampleCreateCommand()
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
        response.Should().BeOfType<ExampleCreateResponse>();
    }

    [Fact]
    public async Task Given_RequestIsValid_When_ExampleDeleteCommandIsCalled_Then_ReturnTypeIsCorrect()
    {
        // Arrange
        await testData.AddExampleEntitiesToDatabase(5);

        var controller = GetSystemUnderTest();
        var handler = ServiceProvider.GetRequiredService<ExampleDeleteCommandHandler>();

        var request = new ExampleDeleteCommand()
        {
            Id = 3
        };

        // Act
        var response = await controller.ExampleDeleteCommand(handler, request, new CancellationToken());

        // Assert
        response.Should().BeOfType<ExampleDeleteResponse>();
    }
}