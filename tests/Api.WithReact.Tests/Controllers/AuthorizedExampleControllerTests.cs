﻿using DomainModel;
using DomainModel.Commands;
using DomainModel.Queries;
using DomainModel.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using Test.Shared.Mocks;
using Test.Shared.TestData;

namespace Api.WithReact.Tests.Controllers;

[Collection("Controller")]
public class AuthorizedExampleControllerTests : ControllerTest {

    [Fact]
    public async Task Given_RequestIsValid_When_ExampleQueryIsCalled_Then_ReturnTypeIsCorrect() {
        // Arrange
        var transaction = ServiceProvider.GetService<DomainModelTransaction>();
        transaction.ResetExampleEntities();

        var testData = new ExampleTestData(transaction);
        await testData.AddExampleEntitiesToDatabase(5);

        var controller = new AuthorizedExampleController();
        var handler = new ExampleLoadSingleQueryHandler(transaction, new ExampleReverseStringService());

        int exampleEntityId = 3;

        // Act
        var response = await controller.ExampleQuery(handler, exampleEntityId, new CancellationToken());

        // Assert
        response.Value.Should().BeOfType<ExampleLoadSingleResponse>();
    }

    [Fact]
    public async Task Given_RequestIsValid_When_ExampleQueryAllIsCalled_Then_ReturnTypeIsCorrect() {
        // Arrange
        var transaction = ServiceProvider.GetService<DomainModelTransaction>();
        transaction.ResetExampleEntities();

        var testData = new ExampleTestData(transaction);
        await testData.AddExampleEntitiesToDatabase(5);

        var controller = new AuthorizedExampleController();
        var handler = new ExampleLoadAllQueryHandler(transaction);

        // Act
        var response = await controller.ExampleQueryAll(handler, new CancellationToken());

        // Assert
        response.Value.Should().BeOfType<ExampleLoadAllResponse>();
    }

    [Fact]
    public async Task Given_RequestIsValid_When_ExampleCreateCommandIsCalled_Then_ReturnTypeIsCorrect() {
        // Arrange
        var transaction = ServiceProvider.GetService<DomainModelTransaction>();
        transaction.ResetExampleEntities();

        var exampleHubMock = new ExampleHubMock();

        var controller = new AuthorizedExampleController();
        var handler = new ExampleCreateCommandHandler(transaction, exampleHubMock);

        var request = new ExampleCreateCommand() {
            Name = "Integration",
            Description = "Test",
            Email = "integration@test.net",
            Phone = "+45 70 70 70 70"
        };

        var cancellationToken = new CancellationToken();

        // Act
        var response = await controller.ExampleCreateCommand(handler, request, cancellationToken);

        // Assert
        response.Value.Should().BeOfType<ExampleCreateResponse>();
    }

    [Fact]
    public async Task Given_RequestIsValid_When_ExampleDeleteCommandIsCalled_Then_ReturnTypeIsCorrect() {
        // Arrange
        var transaction = ServiceProvider.GetService<DomainModelTransaction>();
        transaction.ResetExampleEntities();

        var testData = new ExampleTestData(transaction);
        await testData.AddExampleEntitiesToDatabase(5);

        var controller = new AuthorizedExampleController();
        var handler = new ExampleDeleteCommandHandler(transaction, new ExampleHubMock());

        var request = new ExampleDeleteCommand() {
            Id = 3
        };

        // Act
        var response = await controller.ExampleDeleteCommand(handler, request, new CancellationToken());

        // Assert
        response.Value.Should().BeOfType<ExampleDeleteResponse>();
    }
}