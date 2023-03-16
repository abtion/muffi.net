using DomainModel;
using DomainModel.Example.Commands;
using DomainModel.Example.Queries;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Test.Shared.TestData;

namespace Api.WithReact.Tests.Controllers;

[Collection("Controller")]
public class AuthorizedExampleControllerTests : ControllerTest<AuthorizedExampleController>
{
    public AuthorizedExampleControllerTests()
    {
        transaction = ServiceProvider.GetRequiredService<DomainModelTransaction>();
        transaction.ResetExampleEntities();

        testData = ServiceProvider.GetRequiredService<ExampleTestData>();
    }

    private readonly DomainModelTransaction transaction;
    private readonly ExampleTestData testData;

    protected override AuthorizedExampleController GetSystemUnderTest()
    {
        return new AuthorizedExampleController();
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
        var response = await controller.ExampleQuery(handler, exampleEntityId, new());

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
        var response = await controller.ExampleQueryAll(handler, new());

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

        // Act
        var response = await controller.ExampleCreateCommand(handler, request, new());

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
        var response = await controller.ExampleDeleteCommand(handler, request, new());

        // Assert
        response.Should().BeOfType<ExampleDeleteResponse>();
    }
}