using FluentAssertions;
using MuffiNet.Backend.DomainModel.Queries.ExampleQueryAll;
using MuffiNet.Test.Shared.TestData;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MuffiNet.Backend.Tests.DomainModel.Queries.ExampleQueryAll;

[Collection("ExampleCollection")]
public class ExampleQueryAllTests : DomainModelTest<ExampleQueryAllHandler> {
    private ExampleTestData exampleTestData;

    public ExampleQueryAllTests() {
        // uses static member as database - that needs to be flushed with every test
        Transaction.ResetExampleEntities();

        exampleTestData = new ExampleTestData(Transaction);
    }

    protected async internal override Task<ExampleQueryAllHandler> CreateSut() {
        var sut = new ExampleQueryAllHandler(Transaction);

        return await Task.FromResult(sut);
    }

    private ExampleQueryAllRequest CreateValidRequest() {
        return new ExampleQueryAllRequest();
    }

    #region "Guard Tests"
    [Fact]
    public async void Given_DomainModelTransactionIsNull_When_HandlerIsConstructed_Then_AnArgumentNullExceptionIsThrown() {
        await CreateSut();
        Func<ExampleQueryAllHandler> f = () => new ExampleQueryAllHandler(null);

        f.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async Task Given_RequestIsNull_When_HandleIsCalled_Then_AnArgumentNullExceptionIsThrown() {
        var sut = await CreateSut();

        Task result() => sut.Handle(null, new CancellationToken());

        await Assert.ThrowsAsync<ArgumentNullException>(result);
    }
    #endregion "Guard Tests"

    #region "Happy Path Tests"
    [Fact]
    public async Task Given_DatabaseIsEmpty_When_HandlerIsCalled_Then_EmptyListIsReturned() {
        var sut = await CreateSut();

        var response = await sut.Handle(CreateValidRequest(), new CancellationToken());

        response.ExampleEntities.Should().NotBeNull();
        response.ExampleEntities.Should().BeEmpty();
    }

    [Fact]
    public async Task Given_EntitiesInTheDatabase_When_HandlerIsCalled_Then_TheyAreReturned() {
        await exampleTestData.AddExampleEntitiesToDatabase(3);
        var sut = await CreateSut();

        var response = await sut.Handle(CreateValidRequest(), new CancellationToken());

        response.ExampleEntities.Should().NotBeNull();
        response.ExampleEntities.Count.Should().Be(3);
    }
    #endregion "Happy Path Tests"
}