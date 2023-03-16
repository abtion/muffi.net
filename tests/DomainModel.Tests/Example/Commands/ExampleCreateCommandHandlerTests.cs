using DomainModel.Example.Commands;
using Microsoft.Extensions.DependencyInjection;
using Test.Shared.TestData;

namespace DomainModel.Tests.Example.Commands;

[Collection("ExampleCollection")]
public class ExampleCreateCommandHandlerTests : DomainModelTest<ExampleCreateCommandHandler>
{
    public ExampleCreateCommandHandlerTests()
    {
        transaction = ServiceProvider.GetRequiredService<DomainModelTransaction>();
        transaction.ResetExampleEntities();

        testData = ServiceProvider.GetRequiredService<ExampleTestData>();
    }

    private readonly DomainModelTransaction transaction;
    private readonly ExampleTestData testData;

    private ExampleCreateCommand CreateValidCommand()
    {
        return new ExampleCreateCommand()
        {
            Name = "Muffi",
            Description = "Head of People",
            Email = "a@b.c",
            Phone = "89898989"
        };
    }

    [Fact]
    public async void Given_RequestIsValid_When_HandlerIsCalled_Then_TheEntityIsReturned()
    {
        var command = CreateValidCommand();
        var sut = GetSystemUnderTest();

        var result = await sut.Handle(command, new());

        result.ExampleEntity.Should().NotBeNull();
    }

    [Fact]
    public async void Given_RequestIsValid_When_HandlerIsCalled_Then_TheEntityIsStored()
    {
        var command = CreateValidCommand();
        var sut = GetSystemUnderTest();

        var result = await sut.Handle(command, new());

        transaction.ExampleEntities().WithId(result.ExampleEntity.Id).Should().HaveCount(1);
    }

    [Fact]
    public async void Given_RequestIsValid_When_HandlerIsCalled_Then_NameMatches()
    {
        var command = CreateValidCommand();
        var sut = GetSystemUnderTest();

        var result = await sut.Handle(command, new());

        result.ExampleEntity.Name.Should().Be(command.Name);
    }

    [Fact]
    public async void Given_RequestIsValid_When_HandlerIsCalled_Then_DescriptionMatches()
    {
        var command = CreateValidCommand();
        var sut = GetSystemUnderTest();

        var result = await sut.Handle(command, new());

        result.ExampleEntity.Description.Should().Be(command.Description);
    }

    [Fact]
    public async void Given_RequestIsValid_When_HandlerIsCalled_Then_IdHasAPositiveValue()
    {
        var command = CreateValidCommand();
        var sut = GetSystemUnderTest();

        var result = await sut.Handle(command, new());

        result.ExampleEntity.Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async void Given_TwoRequestsAreSent_Then_BothAreStored()
    {
        var sut = GetSystemUnderTest();
        var command = CreateValidCommand();

        await sut.Handle(command, new());
        await sut.Handle(command, new());

        transaction.ExampleEntities().Should().HaveCount(2);
    }
}