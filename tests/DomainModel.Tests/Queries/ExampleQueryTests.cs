using FluentAssertions;
using DomainModel.Queries.ExampleQuery;
using DomainModel.Exceptions;
using DomainModel.Services;
using Test.Shared.TestData;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DomainModel.Tests.Queries;

[Collection("ExampleCollection")]
public class ExampleQueryTests : DomainModelTest<ExampleQueryHandler>
{
    private ExampleTestData exampleTestData;
    private ExampleReverseStringService exampleReverseStringService;
    public ExampleQueryTests()
    {
        // uses static member as database - that needs to be flushed with every test
        Transaction.ResetExampleEntities();

        exampleTestData = new ExampleTestData(Transaction);
    }

    protected async internal override Task<ExampleQueryHandler> CreateSut()
    {
        exampleReverseStringService = new ExampleReverseStringService();
        var sut = new ExampleQueryHandler(Transaction, exampleReverseStringService);

        return await Task.FromResult(sut);
    }

    private ExampleQueryRequest CreateValidRequest()
    {
        var request = new ExampleQueryRequest()
        {
            Id = 3
        };

        return request;
    }

    #region "Guard Tests"
    [Fact]
    public async void Given_DomainModelTransactionIsNull_When_HandlerIsConstructed_Then_AnArgumentNullExceptionIsThrown()
    {
        await CreateSut();
        Func<ExampleQueryHandler> f = () => new ExampleQueryHandler(null, exampleReverseStringService);

        f.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async void Given_ExampleReverseStringServiceIsNull_When_HandlerIsConstructed_Then_AnArgumentNullExceptionIsThrown()
    {
        await CreateSut();
        Func<ExampleQueryHandler> f = () => new ExampleQueryHandler(Transaction, null);

        f.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async Task Given_RequestIsNull_When_HandleIsCalled_Then_AnArgumentNullExceptionIsThrown()
    {
        var sut = await CreateSut();

        Task result() => sut.Handle(null, new CancellationToken());

        await Assert.ThrowsAsync<ArgumentNullException>(result);
    }
    #endregion "Guard Tests"

    #region "Happy Path Tests"
    [Fact]
    public async Task Given_EntityWithTheGivenIdIsInTheDatabase_When_HandlerIsCalled_Then_EntityIsReturned()
    {
        await exampleTestData.AddExampleEntitiesToDatabase(5);

        var sut = await CreateSut();

        var response = await sut.Handle(CreateValidRequest(), new CancellationToken());

        response.ExampleEntity.Should().NotBeNull();
    }

    [Fact]
    public async Task Given_EntityWithTheGivenIdIsNotInTheDatabase_When_HandlerIsCalled_Then_AnExceptionIsThrown()
    {
        await exampleTestData.AddExampleEntitiesToDatabase(1);

        var sut = await CreateSut();

        Func<Task> act = async () => await sut.Handle(CreateValidRequest(), new CancellationToken());

        await act.Should().ThrowAsync<ExampleEntityNotFoundException>();
    }
    #endregion "Happy Path Tests"
}