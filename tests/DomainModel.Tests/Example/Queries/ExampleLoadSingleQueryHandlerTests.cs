using DomainModel.Example.Queries;
using DomainModel.Shared.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Test.Shared.TestData;

namespace DomainModel.Tests.Example.Queries;

[Collection("ExampleCollection")]
public class ExampleLoadSingleQueryHandlerTests : DomainModelTest<ExampleLoadSingleQueryHandler>
{
    public ExampleLoadSingleQueryHandlerTests()
    {
        transaction = ServiceProvider.GetRequiredService<DomainModelTransaction>();
        transaction.ResetExampleEntities();

        testData = ServiceProvider.GetRequiredService<ExampleTestData>();
    }

    private readonly DomainModelTransaction transaction;
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
        await testData.AddExampleEntitiesToDatabase(5);
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
        await testData.AddExampleEntitiesToDatabase(1);
        var sut = GetSystemUnderTest();

        // act
        Func<Task> act = async () => await sut.Handle(CreateValidQuery(), new());

        // assert
        await act.Should().ThrowAsync<EntityNotFoundException>();
    }
}