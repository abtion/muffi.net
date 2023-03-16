using DomainModel.Data.Models;
using DomainModel.Example.Commands;
using Microsoft.Extensions.DependencyInjection;
using Test.Shared.TestData;

namespace DomainModel.Tests.Example.Commands;

[Collection("ExampleCollection")]
public class ExampleDeleteCommandHandlerTests : DomainModelTest<ExampleDeleteCommandHandler>
{
    public ExampleDeleteCommandHandlerTests()
    {
        transaction = ServiceProvider.GetRequiredService<DomainModelTransaction>();
        transaction.ResetExampleEntities();

        transaction.AddExampleEntity(new ExampleEntity()
        {
            Id = 10,
            Name = "Muffi",
            Description = "Head of People",
            Phone = "89898989"
        });

        testData = ServiceProvider.GetRequiredService<ExampleTestData>();
    }

    private readonly DomainModelTransaction transaction;
    private readonly ExampleTestData testData;

    private ExampleDeleteCommand CreateValidCommand()
    {
        var request = new ExampleDeleteCommand()
        {
            Id = 10,
        };

        return request;
    }

    [Fact]
    public async void Given_RequestIsValid_When_HandlerIsCalled_Then_TheEntityIsRemoved()
    {
        var command = CreateValidCommand();
        var sut = GetSystemUnderTest();

        var result = await sut.Handle(command, new());

        transaction.ExampleEntities().Should().BeEmpty();
    }

    [Fact]
    public async void Given_RequestIsValid_When_HandlerIsCalled_Then_TheReturnTypeIsNotNull()
    {
        var command = CreateValidCommand();
        var sut = GetSystemUnderTest();

        var result = await sut.Handle(command, new());

        result.Should().NotBeNull();
    }
}