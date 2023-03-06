using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MuffiNet.Backend.DomainModel;
using MuffiNet.Backend.DomainModel.Commands.ExampleCreateCommand;
using MuffiNet.Test.Shared.Mocks;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MuffiNet.Backend.Tests.DomainModel.Commands.ExampleCreateCommand;

[Collection("ExampleCollection")]
public class ExampleCreateCommandTests : DomainModelTest<ExampleCreateCommandHandler> {
    private ExampleHubMock exampleHub;
    private DomainModelTransaction domainModelTransaction;

    protected internal override async Task<ExampleCreateCommandHandler> CreateSut() {
        domainModelTransaction = ServiceProvider.GetService<DomainModelTransaction>();

        // uses static member as database - that needs to be flushed with every test
        domainModelTransaction.ResetExampleEntities();

        exampleHub = new ExampleHubMock();

        return await Task.FromResult(new ExampleCreateCommandHandler(domainModelTransaction, exampleHub));
    }

    private ExampleCreateCommandRequest CreateValidRequest() {
        var request = new ExampleCreateCommandRequest() {
            Name = "Muffi",
            Description = "Head of People"
        };

        return request;
    }

    #region "Guard Tests"
    [Fact]
    public async void Given_DomainModelTransactionIsNull_When_HandlerIsConstructed_Then_AnArgumentNullExceptionIsThrown() {
        await CreateSut();
        Func<ExampleCreateCommandHandler> f = () => new ExampleCreateCommandHandler(null, exampleHub);

        f.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async void Given_ExampleHubIsNull_When_HandlerIsConstructed_Then_AnArgumentNullExceptionIsThrown() {
        await CreateSut();
        Func<ExampleCreateCommandHandler> f = () => new ExampleCreateCommandHandler(domainModelTransaction, null);

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
    public async void Given_RequestIsValid_When_HandlerIsCalled_Then_TheEntityIsReturned() {
        var request = CreateValidRequest();
        var cancellationToken = new CancellationToken();
        var sut = await CreateSut();

        var result = await sut.Handle(request, cancellationToken);

        result.ExampleEntity.Should().NotBeNull();
        exampleHub.EntityCreatedMessageCounter.Should().Be(1);
    }

    [Fact]
    public async void Given_RequestIsValid_When_HandlerIsCalled_Then_TheEntityIsStored() {
        var request = CreateValidRequest();
        var cancellationToken = new CancellationToken();
        var sut = await CreateSut();

        var result = await sut.Handle(request, cancellationToken);

        domainModelTransaction.ExampleEntities().WithId(result.ExampleEntity.Id).Should().HaveCount(1);
    }

    [Fact]
    public async void Given_RequestIsValid_When_HandlerIsCalled_Then_NameMatches() {
        var request = CreateValidRequest();
        var cancellationToken = new CancellationToken();
        var sut = await CreateSut();

        var result = await sut.Handle(request, cancellationToken);

        result.ExampleEntity.Name.Should().Be(request.Name);
    }

    [Fact]
    public async void Given_RequestIsValid_When_HandlerIsCalled_Then_DescriptionMatches() {
        var request = CreateValidRequest();
        var cancellationToken = new CancellationToken();
        var sut = await CreateSut();

        var result = await sut.Handle(request, cancellationToken);

        result.ExampleEntity.Description.Should().Be(request.Description);
    }

    [Fact]
    public async void Given_RequestIsValid_When_HandlerIsCalled_Then_IdHasAPositiveValue() {
        var request = CreateValidRequest();
        var cancellationToken = new CancellationToken();
        var sut = await CreateSut();

        var result = await sut.Handle(request, cancellationToken);

        result.ExampleEntity.Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async void Given_TwoRequestsAreSent_Then_BothAreStored() {
        var cancellationToken = new CancellationToken();
        var sut = await CreateSut();
        var request = CreateValidRequest();

        await sut.Handle(request, cancellationToken);
        await sut.Handle(request, cancellationToken);

        domainModelTransaction.ExampleEntities().Should().HaveCount(2);
    }
    #endregion "Happy Path Tests"

}