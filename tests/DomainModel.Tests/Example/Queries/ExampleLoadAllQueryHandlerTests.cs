using DomainModel.Example.Queries;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using Test.Shared.TestData;

namespace DomainModel.Tests.Example.Queries;

[Collection("ExampleCollection")]
public class ExampleLoadAllQueryHandlerTests : DomainModelTest<ExampleLoadAllQueryHandler>
{
    public ExampleLoadAllQueryHandlerTests()
    {
        transaction = ServiceProvider.GetRequiredService<DomainModelTransaction>();
        transaction.ResetExampleEntities();

        testData = ServiceProvider.GetRequiredService<ExampleTestData>();
    }

    private readonly DomainModelTransaction transaction;
    private readonly ExampleTestData testData;

    private ExampleLoadAllQuery CreateValidQuery()
    {
        return new ExampleLoadAllQuery();
    }

    [Fact]
    public async Task Given_DatabaseIsEmpty_When_HandlerIsCalled_Then_EmptyListIsReturned()
    {
        // arrange
        var sut = GetSystemUnderTest();

        // act
        var response = await sut.Handle(CreateValidQuery(), new CancellationToken());

        // assert
        response.ExampleEntities.Should().NotBeNull();
        response.ExampleEntities.Should().BeEmpty();
    }

    [Fact]
    public async Task Given_EntitiesInTheDatabase_When_HandlerIsCalled_Then_TheyAreReturned()
    {
        // arrange
        await testData.AddExampleEntitiesToDatabase(3);
        var sut = GetSystemUnderTest();

        // act
        var response = await sut.Handle(CreateValidQuery(), new CancellationToken());

        // assert
        response.ExampleEntities.Should().NotBeNull();
        response.ExampleEntities.Count.Should().Be(3);
    }
}