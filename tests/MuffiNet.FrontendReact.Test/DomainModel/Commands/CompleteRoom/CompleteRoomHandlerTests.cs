using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MuffiNet.FrontendReact.DomainModel;
using MuffiNet.FrontendReact.DomainModel.Commands.CompleteRoom;
using MuffiNet.FrontendReact.Exceptions;
using MuffiNet.FrontendReact.Models;
using MuffiNet.FrontendReact.Services;
using MuffiNet.FrontendReact.Test.Mocks;
using Xunit;

namespace MuffiNet.FrontendReact.Test.DomainModel.Commands.CompleteRoom
{
    public class CompleteRoomHandlerTests : DomainModelTest<CompleteRoomHandler>
    {
        private readonly string supportTicketId = "EAA147BF-2D9A-4F41-BADE-17085AB464DF";
        private readonly string supportTicketIdWithCompletedRoom = "BCD147BF-2D9A-4F41-BADE-17085AB464DF";
        private CustomerHubMock customerHub;
        private TechnicianHubMock technicianHub;

        private string RoomName { get { return $"room_{supportTicketId.ToLower()}"; } }
        private string CompletedRoomName { get { return $"room_{supportTicketIdWithCompletedRoom.ToLower()}"; } }
        private string TwilioRoomSid { get { return "RM2124b63d675b17ad52b8af15d2bc511d"; } }
        private string CompletedTwilioRoomSid { get { return "COMPLETED3d675b17ad52b8af15d2bc511d"; } }

        private Mock<ITwilioService> twilioServiceMock { get; set; }

        protected internal override async Task<CompleteRoomHandler> CreateSut()
        {
            var domainModelTransaction = ServiceProvider.GetService<DomainModelTransaction>();

            customerHub = new CustomerHubMock();
            technicianHub = new TechnicianHubMock();

            twilioServiceMock = new Mock<ITwilioService>();
            twilioServiceMock.Setup(p => p.IsRoomCompleted(TwilioRoomSid)).Returns(Task.FromResult(false));
            twilioServiceMock.Setup(p => p.IsRoomCompleted(CompletedTwilioRoomSid)).Returns(Task.FromResult(true));
            twilioServiceMock.Setup(p => p.IsRoomCompleted(null)).Throws(new NullReferenceException());
            twilioServiceMock.Setup(p => p.IsRoomCompleted("wrongSid")).Throws(new TwilioRoomNotFoundException($"TwilioRoom with room sid {"wrongSid"} was not found"));
            twilioServiceMock.Setup(p => p.CompleteRoom(RoomName));

            return await Task.FromResult(new CompleteRoomHandler(domainModelTransaction, CurrentDateTimeServiceMock.MockCurrentDateTimeService(), customerHub, technicianHub, twilioServiceMock.Object));
        }

        private CompleteRoomRequest CreateValidRequest()
        {
            var request = new CompleteRoomRequest()
            {
                SupportTicketId = supportTicketId,
            };

            return request;
        }

        [Fact]
        public async void Given_RequestIsValid_When_CompleteRoomHandlerIsCalled_Then_TheSupportTicketIsUpdated()
        {
            var request = CreateValidRequest();
            var cancellationToken = new CancellationToken();
            var sut = await CreateSut();

            var transaction = ServiceProvider.GetService<DomainModelTransaction>();
            await TestData.CreateSupportTicketWithTwilioRoom(new Guid(request.SupportTicketId));
            var currentUser = await (new CurrentUserServiceMock()).CurrentUser();

            var result = await sut.Handle(request, cancellationToken);


            var supportTicket = transaction.Entities<SupportTicket>().Where(p => p.SupportTicketId == new Guid(request.SupportTicketId)).SingleOrDefault();
            Assert.Equal(CurrentDateTimeServiceMock.MockedDateTime(), supportTicket.CallEndedAt);
        }

        [Fact]
        public async void Given_SupportTicketNotInDatabase_When_CompleteRoomHandlerIsCalled_Then_SupportTicketNotFoundExceptionIsThrown()
        {
            var request = CreateValidRequest();
            var cancellationToken = new CancellationToken();
            var sut = await CreateSut();

            var currentUser = await (new CurrentUserServiceMock()).CurrentUser();

            await Assert.ThrowsAsync<SupportTicketNotFoundException>(() => sut.Handle(request, cancellationToken));
        }

        [Fact]
        public async void Given_SupportTicketIdIsNotAGuid_When_CompleteRoomHandlerIsCalled_Then_SupportTicketIdInvalidExceptionIsThrown()
        {
            var request = CreateValidRequest();
            request.SupportTicketId = "NotAGuid";

            var cancellationToken = new CancellationToken();
            var sut = await CreateSut();

            var currentUser = await (new CurrentUserServiceMock()).CurrentUser();

            await Assert.ThrowsAsync<SupportTicketIdInvalidException>(() => sut.Handle(request, cancellationToken));
        }

        [Fact]
        public async void Given_RequestIsValid_When_CompleteRoomHandlerIsCalled_Then_AMessageIsSentToTheCustomer()
        {
            var request = CreateValidRequest();
            var cancellationToken = new CancellationToken();
            var sut = await CreateSut();

            await TestData.CreateSupportTicketWithTwilioRoom(new Guid(request.SupportTicketId));

            await sut.Handle(request, cancellationToken);

            Assert.Equal(1, customerHub.TechnicianHasEndedCallCounter);
            Assert.Equal(request.SupportTicketId, customerHub.LatestEndedCallMessage.SupportTicketId);

            Assert.Equal(1, technicianHub.SupportTicketDeletedMessageCounter);
            Assert.Equal(request.SupportTicketId, technicianHub.LatestSupportTicketDeletedMessage.SupportTicketId);
        }

        [Fact]
        public async void Given_TwiliocRoomIsAlreadyCompleted_When_CompleteRoomHandlerIsCalled_Then_CompleteRoomIsNotCalled()
        {
            // arrange
            var cancellationToken = new CancellationToken();
            var sut = await CreateSut();
            var request = CreateValidRequest();
            request.SupportTicketId = supportTicketIdWithCompletedRoom;

            // act
            await TestData.CreateSupportTicketWithTwilioRoom(new Guid(request.SupportTicketId), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), CompletedTwilioRoomSid);
            await sut.Handle(request, cancellationToken);

            // assert
            twilioServiceMock.Verify(mock => mock.IsRoomCompleted(CompletedTwilioRoomSid), Times.Once);
            twilioServiceMock.Verify(mock => mock.CompleteRoom(RoomName), Times.Never);
        }

        [Fact]
        public async void Given_TwilioRoomIsNotCompleted_When_CompleteRoomHandlerIsCalled_Then_CompleteRoomIsCalled()
        {
            // arrange
            var cancellationToken = new CancellationToken();
            var sut = await CreateSut();
            var request = CreateValidRequest();

            // act
            await TestData.CreateSupportTicketWithTwilioRoom(new Guid(request.SupportTicketId), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), TwilioRoomSid);
            await sut.Handle(request, cancellationToken);

            // assert
            twilioServiceMock.Verify(mock => mock.IsRoomCompleted(TwilioRoomSid), Times.Once);
            twilioServiceMock.Verify(mock => mock.CompleteRoom(RoomName), Times.Once);
        }
        [Fact]
        public async void Given_TwilioRoomSidIsNotSetOnSupportTicket_When_CompleteRoomIsCalledompleteRoomHandlerIsCalled_Then_ANullReferenceExceptionIsThrown()
        {
            // arrange
            var cancellationToken = new CancellationToken();
            var sut = await CreateSut();
            var request = CreateValidRequest();

            // act
            await TestData.CreateSupportTicketWithTwilioRoom(new Guid(request.SupportTicketId), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), null);

            // assert
            await Assert.ThrowsAsync<NullReferenceException>(() => sut.Handle(request, cancellationToken));
        }
        [Fact]
        public async void Given_TwilioRoomSidIsNotCorrectOnSupportTicket_When_CompleteRoomIsCalledompleteRoomHandlerIsCalled_Then_ATwilioRoomNotFoundExceptionIsThrown()
        {
            // arrange
            var cancellationToken = new CancellationToken();
            var sut = await CreateSut();
            var request = CreateValidRequest();

            // act
            await TestData.CreateSupportTicketWithTwilioRoom(new Guid(request.SupportTicketId), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "wrongSid");

            // assert
            await Assert.ThrowsAsync<TwilioRoomNotFoundException>(() => sut.Handle(request, cancellationToken));
        }
    }
}
