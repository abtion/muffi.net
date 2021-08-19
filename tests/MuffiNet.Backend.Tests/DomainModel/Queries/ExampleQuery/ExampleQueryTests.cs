using FluentAssertions;
using MuffiNet.Backend.DomainModel.Queries.ExampleQuery;
using MuffiNet.Backend.Exceptions;
using MuffiNet.Test.Shared.TestData;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MuffiNet.Backend.Tests.DomainModel.Queries.ExampleQuery
{
    public class ExampleQueryTests : DomainModelTest<ExampleQueryHandler>
    {
        private ExampleTestData exampleTestData;

        public ExampleQueryTests()
        {
            // uses static member as database - that needs to be flushed with every test
            Transaction.ResetExampleEntities();

            exampleTestData = new ExampleTestData(Transaction);
        }

        protected async internal override Task<ExampleQueryHandler> CreateSut()
        {
            var sut = new ExampleQueryHandler(Transaction);

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

            act.Should().Throw<ExampleEntityNotFoundException>();
        }
    }
}