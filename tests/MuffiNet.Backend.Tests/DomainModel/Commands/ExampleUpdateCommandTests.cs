using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MuffiNet.Backend.DomainModel;
using MuffiNet.Backend.DomainModel.Commands.ExampleUpdateCommand;
using MuffiNet.Backend.Exceptions;
using MuffiNet.Backend.Models;
using MuffiNet.Test.Shared.Mocks;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MuffiNet.Backend.Tests.DomainModel.Commands.ExampleUpdateCommand
{
    [Collection("ExampleCollection")]
    public class ExampleUpdateCommandTests : DomainModelTest<ExampleUpdateCommandHandler>
    {
        private ExampleHubMock exampleHub;
        private DomainModelTransaction domainModelTransaction;

        protected internal override async Task<ExampleUpdateCommandHandler> CreateSut()
        {
            domainModelTransaction = ServiceProvider.GetService<DomainModelTransaction>();

            // uses static member as database - that needs to be flushed with every test
            domainModelTransaction.ResetExampleEntities();
            domainModelTransaction.AddExampleEntity(new ExampleEntity()
            {
                Id = 10,
                Name = "Muffi",
                Description = "Head of People",
                Phone = "12348765",
                Email = "muffi@abtion.com"
            });
            exampleHub = new ExampleHubMock();

            return await Task.FromResult(new ExampleUpdateCommandHandler(domainModelTransaction, exampleHub));
        }

        private ExampleUpdateCommandRequest CreateValidRequest()
        {
            var request = new ExampleUpdateCommandRequest()
            {
                Id = 10,
                Name = "MuffiNew",
                Description = "Head of Dogs",
                Phone = "56784321",
                Email = "iffum@abtion.com"
            };

            return request;
        }

        #region "Guard Tests"
        [Fact]
        public async void Given_DomainModelTransactionIsNull_When_HandlerIsConstructed_Then_AnArgumentNullExceptionIsThrown()
        {
            await CreateSut();
            Func<ExampleUpdateCommandHandler> f = () => new ExampleUpdateCommandHandler(null, exampleHub);

            f.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async void Given_ExampleHubIsNull_When_HandlerIsConstructed_Then_AnArgumentNullExceptionIsThrown()
        {
            await CreateSut();
            Func<ExampleUpdateCommandHandler> f = () => new ExampleUpdateCommandHandler(domainModelTransaction, null);

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
        public async void Given_RequestIsValid_When_HandlerIsCalled_Then_TheEntityIsUpdatedAndReturned()
        {
            var request = CreateValidRequest();
            var cancellationToken = new CancellationToken();
            var sut = await CreateSut();

            var result = await sut.Handle(request, cancellationToken);

            result.ExampleEntity.Should().NotBeNull();
            result.ExampleEntity.Id.Should().Be(10);
            result.ExampleEntity.Name.Should().Be("MuffiNew");
            result.ExampleEntity.Description.Should().Be("Head of Dogs");
            result.ExampleEntity.Phone.Should().Be("56784321");
            result.ExampleEntity.Email.Should().Be("iffum@abtion.com");
            exampleHub.EntityUpdatedMessageCounter.Should().Be(1);
        }
        #endregion "Happy Path Tests"

        [Fact]
        public async void Given_EntityDoesNotExist_When_HandlerIsCalled_Then_AnExceptionIsThrown()
        {
            var request = CreateValidRequest();
            var sut = await CreateSut();
            request.Id = 1234;
            Task result() => sut.Handle(request, new CancellationToken());

            await Assert.ThrowsAsync<ExampleEntityNotFoundException>(result);

        }

    }
}