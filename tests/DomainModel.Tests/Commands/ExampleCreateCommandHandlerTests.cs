using DomainModel.Commands;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using Test.Shared.Mocks;
using Xunit;

namespace DomainModel.Tests.Commands;

[Collection("ExampleCollection")]
public class ExampleCreateCommandHandlerTests : DomainModelTest<ExampleCreateCommandHandler> {
    private DomainModelTransaction domainModelTransaction;

    protected internal override async Task<ExampleCreateCommandHandler> CreateSut() {
        domainModelTransaction = ServiceProvider.GetService<DomainModelTransaction>();

        // uses static member as database - that needs to be flushed with every test
        domainModelTransaction.ResetExampleEntities();

        return await Task.FromResult(new ExampleCreateCommandHandler(domainModelTransaction));
    }

    private ExampleCreateCommand CreateValidCommand() {
        var request = new ExampleCreateCommand() {
            Name = "Muffi",
            Description = "Head of People",
            Email = "a@b.c",
            Phone = "89898989"
        };

        return request;
    }

    #region "Guard Tests"
    [Fact]
    public async void Given_DomainModelTransactionIsNull_When_HandlerIsConstructed_Then_AnArgumentNullExceptionIsThrown() {
        await CreateSut();
        Func<ExampleCreateCommandHandler> f = () => new ExampleCreateCommandHandler(null);

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
        var request = CreateValidCommand();
        var cancellationToken = new CancellationToken();
        var sut = await CreateSut();

        var result = await sut.Handle(request, cancellationToken);

        result.ExampleEntity.Should().NotBeNull();
    }

    [Fact]
    public async void Given_RequestIsValid_When_HandlerIsCalled_Then_TheEntityIsStored() {
        var request = CreateValidCommand();
        var cancellationToken = new CancellationToken();
        var sut = await CreateSut();

        var result = await sut.Handle(request, cancellationToken);

        domainModelTransaction.ExampleEntities().WithId(result.ExampleEntity.Id).Should().HaveCount(1);
    }

    [Fact]
    public async void Given_RequestIsValid_When_HandlerIsCalled_Then_NameMatches() {
        var request = CreateValidCommand();
        var cancellationToken = new CancellationToken();
        var sut = await CreateSut();

        var result = await sut.Handle(request, cancellationToken);

        result.ExampleEntity.Name.Should().Be(request.Name);
    }

    [Fact]
    public async void Given_RequestIsValid_When_HandlerIsCalled_Then_DescriptionMatches() {
        var request = CreateValidCommand();
        var cancellationToken = new CancellationToken();
        var sut = await CreateSut();

        var result = await sut.Handle(request, cancellationToken);

        result.ExampleEntity.Description.Should().Be(request.Description);
    }

    [Fact]
    public async void Given_RequestIsValid_When_HandlerIsCalled_Then_IdHasAPositiveValue() {
        var request = CreateValidCommand();
        var cancellationToken = new CancellationToken();
        var sut = await CreateSut();

        var result = await sut.Handle(request, cancellationToken);

        result.ExampleEntity.Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async void Given_TwoRequestsAreSent_Then_BothAreStored() {
        var cancellationToken = new CancellationToken();
        var sut = await CreateSut();
        var request = CreateValidCommand();

        await sut.Handle(request, cancellationToken);
        await sut.Handle(request, cancellationToken);

        domainModelTransaction.ExampleEntities().Should().HaveCount(2);
    }
    #endregion "Happy Path Tests"

}