﻿using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MuffiNet.Backend.DomainModel;
using MuffiNet.Backend.DomainModel.Commands.ExampleDeleteCommand;
using MuffiNet.Backend.DomainModel.Commands.ExampleCreateCommand;
using MuffiNet.Test.Shared.Mocks;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using MuffiNet.Backend.Models;

namespace MuffiNet.Backend.Tests.DomainModel.Commands.ExampleDeleteCommand
{
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
        public void Given_DomainModelTransactionIsNull_When_HandlerIsConstructed_Then_AnArgumentNullExceptionIsThrown()
        {
            Func<ExampleDeleteCommandHandler> f = () => new ExampleDeleteCommandHandler(null, exampleHub);

            f.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Given_ExampleHubIsNull_When_HandlerIsConstructed_Then_AnArgumentNullExceptionIsThrown()
        {
            Func<ExampleDeleteCommandHandler> f = () => new ExampleDeleteCommandHandler(domainModelTransaction, null);

            f.Should().Throw<ArgumentNullException>();
        }
        #endregion "Guard Tests"

        #region "Happy Path Tests"
        [Fact]
        public async void Given_RequestIsValid_When_HandlerIsCalled_Then_TheEntityIsRemoved()
        {
            var request = CreateValidRequest();
            var cancellationToken = new CancellationToken();
            var sut = await CreateSut();

            await sut.Handle(request, cancellationToken);

            domainModelTransaction.ExampleEntities().Should().BeEmpty();
            exampleHub.EntityDeletedMessageCounter.Should().Be(1);
        }
        #endregion "Happy Path Tests"
    }
}