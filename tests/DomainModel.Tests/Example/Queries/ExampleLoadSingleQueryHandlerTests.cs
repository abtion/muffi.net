using Presentation.Example.Queries;
using Domain.Shared.Exceptions;
using Test.Shared.TestData;
using static Presentation.Example.Queries.ExampleLoadSingleQueryHandler;

namespace Domain.Tests.Example.Queries;

[Collection("ExampleCollection")]
public class ExampleLoadSingleQueryHandlerTests : DomainModelTest<ExampleLoadSingleQueryHandler>
{
    public ExampleLoadSingleQueryHandlerTests()
    {
        testData = ServiceProvider.GetRequiredService<ExampleTestData>();
    }

    private readonly ExampleTestData testData;

    private ExampleLoadSingleQuery CreateValidQuery()
    {
        var request = new ExampleLoadSingleQuery()
        {
            Id = 3
        };

        return request;
    }

    [Fact]
    public async Task Given_EntityWithTheGivenIdIsInTheDatabase_When_HandlerIsCalled_Then_EntityIsReturned()
    {
        // arrange
        await testData.ResetExampleEntities(new());
        await testData.AddExampleEntitiesToDatabase(5, new());
        var sut = GetSystemUnderTest();

        // act
        var response = await sut.Handle(CreateValidQuery(), new());

        // assert
        response.ExampleEntity.Should().NotBeNull();
    }

    [Fact]
    public async Task Given_EntityWithTheGivenIdIsNotInTheDatabase_When_HandlerIsCalled_Then_AnExceptionIsThrown()
    {
        // arrange
        await testData.ResetExampleEntities(new());
        await testData.AddExampleEntitiesToDatabase(1, new());
        var sut = GetSystemUnderTest();

        // act
        Func<Task> act = async () => await sut.Handle(CreateValidQuery(), new());

        // assert
        await act.Should().ThrowAsync<EntityNotFoundException>();
    }
}