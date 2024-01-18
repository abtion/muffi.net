using Presentation.Example.Queries;
using Test.Shared.TestData;

using static Presentation.Example.Queries.ExampleLoadAllQueryHandler;

namespace Domain.Tests.Example.Queries;

[Collection("ExampleCollection")]
public class ExampleLoadAllQueryHandlerTests : DomainModelTest<ExampleLoadAllQueryHandler>
{
    public ExampleLoadAllQueryHandlerTests()
    {
        testData = ServiceProvider.GetRequiredService<ExampleTestData>();
    }

    private readonly ExampleTestData testData;

    private ExampleLoadAllQuery CreateValidQuery()
    {
        return new ExampleLoadAllQuery();
    }

    [Fact]
    public async Task Given_DatabaseIsEmpty_When_HandlerIsCalled_Then_EmptyListIsReturned()
    {
        // arrange
        await testData.ResetExampleEntities(new());

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
        await testData.ResetExampleEntities(new());
        await testData.AddExampleEntitiesToDatabase(3, new());

        var sut = GetSystemUnderTest();

        // act
        var response = await sut.Handle(CreateValidQuery(), new CancellationToken());

        // assert
        response.ExampleEntities.Should().NotBeNull();
        response.ExampleEntities.Count.Should().Be(3);
    }
}