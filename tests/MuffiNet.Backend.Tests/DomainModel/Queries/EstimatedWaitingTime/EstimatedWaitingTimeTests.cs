using System;
using System.Threading;
using System.Threading.Tasks;
using MuffiNet.Backend.DomainModel.Queries.EstimatedWaitingTime;
using MuffiNet.Backend.Exceptions;
using Xunit;

namespace MuffiNet.Backend.Tests.DomainModel.Queries.EstimatedWaitingTime
{
    public class EstimatedWaitingTimeTests : DomainModelTest<EstimatedWaitingTimeHandler>
    {
        protected internal async override Task<EstimatedWaitingTimeHandler> CreateSut()
        {
            var sut = new EstimatedWaitingTimeHandler(Transaction);

            return await Task.FromResult(sut);
        }

        private string supportTicketId = "45b8403e-c558-431d-8bc7-0b26a154411a";

        [Fact]
        public async Task Given_RequestIsValid_When_HandleIsCalled_Then_TheCorrectNumberOfTicketsIsReturned()
        {
            var sut = await CreateSut();
            var request = new EstimatedWaitingTimeRequest();
            var cancellationToken = new CancellationToken();

            await TestData.CreateDemoSupportTicketsWhereCallHasEnded(Guid.NewGuid().ToString(), new DateTime(2021, 06, 29));
            await TestData.CreateSupportTicketWithInitalFields(Guid.NewGuid().ToString(), new DateTime(2021, 06, 28));
            await TestData.CreateSupportTicketWithInitalFields(Guid.NewGuid().ToString(), new DateTime(2021, 06, 28));
            await TestData.CreateSupportTicketWithInitalFields(Guid.NewGuid().ToString(), new DateTime(2021, 06, 28));

            await TestData.CreateSupportTicketWithInitalFields(supportTicketId, new DateTime(2021, 06, 29));

            await TestData.CreateSupportTicketWithInitalFields(Guid.NewGuid().ToString(), new DateTime(2021, 06, 30));

            request.SupportTicketId = new Guid(supportTicketId);

            var response = await sut.Handle(request, cancellationToken);

            Assert.Equal(EstimatedWaitingTimeHandler.CallDurationInMinutes, response.EstimatedCallDurationInMinutes);
            Assert.Equal(3, response.NumberOfUnansweredCalls);
            Assert.Equal(EstimatedWaitingTimeHandler.CallDurationInMinutes * 3, response.EstimatedWaitingTimeInMinutes);
        }

        [Fact]
        public async Task Given_SupportTicketIsNotFound_When_HandleIsCalled_Then_AnExceptionIsThrown()
        {
            var sut = await CreateSut();
            var request = new EstimatedWaitingTimeRequest();
            request.SupportTicketId = new Guid(supportTicketId);

            var cancellationToken = new CancellationToken();

            await Assert.ThrowsAsync<SupportTicketNotFoundException>(() => sut.Handle(request, cancellationToken));
        }
    }
}