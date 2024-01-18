using Presentation.Example.Queries;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Example.Commands;
using Presentation.Example.Dtos;
using System.Threading;
using System.Threading.Tasks;
using Test.Shared.TestData;

using static Presentation.Example.Queries.ExampleLoadAllQueryHandler;
using static Presentation.Example.Queries.ExampleLoadSingleQueryHandler;

namespace Api.WithReact.Tests.Controllers;

[Collection("Controller")]
public class ExampleControllerTests : ControllerTest<ExampleController>
{
    public ExampleControllerTests()
    {
        testData = ServiceProvider.GetRequiredService<ExampleTestData>();
    }

    private readonly ExampleTestData testData;

    protected override ExampleController GetSystemUnderTest()
    {
        return new ExampleController();
    }

    [Fact]
    public async Task Given_RequestIsValid_When_ExampleQueryIsCalled_Then_ReturnTypeIsCorrect()
    {
        // Arrange
        await testData.ResetExampleEntities(new());
        await testData.AddExampleEntitiesToDatabase(5, new());

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
        await testData.ResetExampleEntities(new());
        await testData.AddExampleEntitiesToDatabase(5, new());

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
        await testData.ResetExampleEntities(new());
        await testData.AddExampleEntitiesToDatabase(5, new());

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