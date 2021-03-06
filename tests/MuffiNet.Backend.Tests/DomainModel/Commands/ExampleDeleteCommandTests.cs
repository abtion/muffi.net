using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MuffiNet.Backend.DomainModel;
using MuffiNet.Backend.DomainModel.Commands.ExampleDeleteCommand;
using MuffiNet.Backend.Models;
using MuffiNet.Test.Shared.Mocks;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MuffiNet.Backend.Tests.DomainModel.Commands.ExampleDeleteCommand;

[Collection("ExampleCollection")]
public class ExampleDeleteCommandTests : DomainModelTest<ExampleDeleteCommandHandler>
{
    private ExampleHubMock exampleHub;
    private DomainModelTransaction domainModelTransaction;

    protected internal override async Task<ExampleDeleteCommandHandler> CreateSut()
    {
        domainModelTransaction = ServiceProvider.GetService<DomainModelTransaction>();

        // uses static member as database - that needs to be flushed with every test
        domainModelTransaction.ResetExampleEntities();
        domainModelTransaction.AddExampleEntity(new ExampleEntity()
        {
            Id = 10,
            Name = "Muffi",
            Description = "Head of People"
        });

        exampleHub = new ExampleHubMock();

        return await Task.FromResult(new ExampleDeleteCommandHandler(domainModelTransaction, exampleHub));
    }

    private ExampleDeleteCommandRequest CreateValidRequest()
    {
        var request = new ExampleDeleteCommandRequest()
        {
            Id = 10,
        };

        return request;
    }

    #region "Guard Tests"
    [Fact]
    public async void Given_DomainModelTransactionIsNull_When_HandlerIsConstructed_Then_AnArgumentNullExceptionIsThrown()
    {
        await CreateSut();
        Func<ExampleDeleteCommandHandler> f = () => new ExampleDeleteCommandHandler(null, exampleHub);

        f.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async void Given_ExampleHubIsNull_When_HandlerIsConstructed_Then_AnArgumentNullExceptionIsThrown()
    {
        await CreateSut();
        Func<ExampleDeleteCommandHandler> f = () => new ExampleDeleteCommandHandler(domainModelTransaction, null);

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
    public async void Given_RequestIsValid_When_HandlerIsCalled_Then_TheEntityIsRemoved()
    {
        var request = CreateValidRequest();
        var cancellationToken = new CancellationToken();
        var sut = await CreateSut();

        var result = await sut.Handle(request, cancellationToken);

        domainModelTransaction.ExampleEntities().Should().BeEmpty();
    }

    [Fact]
    public async void Given_RequestIsValid_When_HandlerIsCalled_Then_TheEntityCountIsReduced()
    {
        var request = CreateValidRequest();
        var cancellationToken = new CancellationToken();
        var sut = await CreateSut();

        var result = await sut.Handle(request, cancellationToken);

        exampleHub.EntityDeletedMessageCounter.Should().Be(1);
    }

    [Fact]
    public async void Given_RequestIsValid_When_HandlerIsCalled_Then_TheReturnTypeIsNotNull()
    {
        var request = CreateValidRequest();
        var cancellationToken = new CancellationToken();
        var sut = await CreateSut();

        var result = await sut.Handle(request, cancellationToken);

        result.Should().NotBeNull();
    }

    #endregion "Happy Path Tests"
}