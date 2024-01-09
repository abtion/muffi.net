using Domain.Example.Commands;
using Domain.Example.Entities;
using Domain.Example.Specifications;
using Domain.Shared;
using Test.Shared.TestData;
using static Domain.Example.Commands.ExampleCreateCommandHandler;

namespace Domain.Tests.Example.Commands;

[Collection("ExampleCollection")]
public class ExampleCreateCommandHandlerTests : DomainModelTest<ExampleCreateCommandHandler>
{
    public ExampleCreateCommandHandlerTests()
    {
        repository = ServiceProvider.GetRequiredService<IRepository<ExampleEntity>>();
        testData = ServiceProvider.GetRequiredService<ExampleTestData>();
    }

    private readonly IRepository<ExampleEntity> repository;
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
        await testData.ResetExampleEntities(new());

        var command = CreateValidCommand();
        var sut = GetSystemUnderTest();

        var result = await sut.Handle(command, new());

        result.ExampleEntity.Should().NotBeNull();
    }

    [Fact]
    public async void Given_RequestIsValid_When_HandlerIsCalled_Then_TheEntityIsStored()
    {
        await testData.ResetExampleEntities(new());

        var command = CreateValidCommand();
        var sut = GetSystemUnderTest();

        var result = await sut.Handle(command, new());

        (await repository.GetAll(new WithId(result.ExampleEntity.Id), new())).Should().HaveCount(1);
    }

    [Fact]
    public async void Given_RequestIsValid_When_HandlerIsCalled_Then_NameMatches()
    {
        await testData.ResetExampleEntities(new());

        var command = CreateValidCommand();
        var sut = GetSystemUnderTest();

        var result = await sut.Handle(command, new());

        result.ExampleEntity.Name.Should().Be(command.Name);
    }

    [Fact]
    public async void Given_RequestIsValid_When_HandlerIsCalled_Then_DescriptionMatches()
    {
        await testData.ResetExampleEntities(new());

        var command = CreateValidCommand();
        var sut = GetSystemUnderTest();

        var result = await sut.Handle(command, new());

        result.ExampleEntity.Description.Should().Be(command.Description);
    }

    [Fact]
    public async void Given_RequestIsValid_When_HandlerIsCalled_Then_IdHasAPositiveValue()
    {
        await testData.ResetExampleEntities(new());

        var command = CreateValidCommand();
        var sut = GetSystemUnderTest();

        var result = await sut.Handle(command, new());

        result.ExampleEntity.Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async void Given_TwoRequestsAreSent_Then_BothAreStored()
    {
        await testData.ResetExampleEntities(new());

        var sut = GetSystemUnderTest();
        var command = CreateValidCommand();

        await sut.Handle(command, new());
        await sut.Handle(command, new());

        (await repository.GetAll(new())).Should().HaveCount(2);
    }
}