using FluentAssertions;
using DomainModel.Exceptions;
using DomainModel.Services;
using Test.Shared.TestData;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using DomainModel.Queries;

namespace DomainModel.Tests.Queries;

[Collection("ExampleCollection")]
public class ExampleQueryTests : DomainModelTest<ExampleLoadSingleQueryHandler>
{
    private ExampleTestData exampleTestData;
    private ExampleReverseStringService exampleReverseStringService;
    public ExampleQueryTests()
    {
        // uses static member as database - that needs to be flushed with every test
        Transaction.ResetExampleEntities();

        exampleTestData = new ExampleTestData(Transaction);
    }

    protected async internal override Task<ExampleLoadSingleQueryHandler> CreateSut()
    {
        exampleReverseStringService = new ExampleReverseStringService();
        var sut = new ExampleLoadSingleQueryHandler(Transaction, exampleReverseStringService);

        return await Task.FromResult(sut);
    }

    private ExampleLoadSingleQuery CreateValidRequest()
    {
        var request = new ExampleLoadSingleQuery()
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
        Func<ExampleLoadSingleQueryHandler> f = () => new ExampleLoadSingleQueryHandler(null, exampleReverseStringService);

        f.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async void Given_ExampleReverseStringServiceIsNull_When_HandlerIsConstructed_Then_AnArgumentNullExceptionIsThrown()
    {
        await CreateSut();
        Func<ExampleLoadSingleQueryHandler> f = () => new ExampleLoadSingleQueryHandler(Transaction, null);

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