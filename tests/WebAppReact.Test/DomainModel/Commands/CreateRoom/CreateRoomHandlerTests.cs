using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebAppReact.DomainModel;
using WebAppReact.DomainModel.Commands.CreateRoom;
using WebAppReact.Exceptions;
using WebAppReact.Models;
using WebAppReact.Services;
using WebAppReact.Test.Mocks;
using WebAppReact.Test.TestData;
using Xunit;

namespace WebAppReact.Test.DomainModel.Commands.CreateRoom
{
    public class CreateRoomHandlerTests : DomainModelTest<CreateRoomHandler>
    {
        private string supportTicketId = "EAA147BF-2D9A-4F41-BADE-17085AB464DF";
        private string technicianToken = "E538F532-7A6A-4E9C-8F6B-0936D7034967";
        private string customerToken = "42E82811-880C-438F-A28D-03D09EED0C37";

        private CustomerHubMock customerHub;
        private TechnicianHubMock technicianHub;

        private string roomName { get { return $"room_{supportTicketId}"; } }
        private string roomSid { get { return $"testsid"; } }

        protected internal async override Task<CreateRoomHandler> CreateSut()
        {
            var mock = new Mock<ITwilioService>();
            mock.Setup(p => p.CreateRoom(roomName)).ReturnsAsync(roomSid);
            mock.Setup(p => p.CreateVideoGrant("Care1 Technician", roomName)).Returns(technicianToken);
            mock.Setup(p => p.CreateVideoGrant("Ruth", roomName)).Returns(customerToken);

            customerHub = new CustomerHubMock();
            technicianHub = new TechnicianHubMock();

            var transaction = ServiceProvider.GetService<DomainModelTransaction>();

            var supportTicketTestData = new SupportTicketTestData(transaction);

            await TestData.CreateSupportTicketWithInitalFields(supportTicketId, DateTime.Now);
            var currentUserServiceMock = new CurrentUserServiceMock();
            var userManager = ServiceProvider.GetService<UserManager<ApplicationUser>>();


            return new CreateRoomHandler(mock.Object, transaction, customerHub, technicianHub, CurrentDateTimeServiceMock.MockCurrentDateTimeService(), currentUserServiceMock, userManager);
        }

        private CreateRoomRequest CreateValidRequest()
        {
            var request = new CreateRoomRequest()
            {
                SupportTicketId = supportTicketId,
            };

            return request;
        }

        [Fact]
        public async void Given_RequestIsValid_When_CreateRoomHandlerIsCalled_Then_ARoomIsCreated_And_TheSupportTicketIsUpdated()
        {
            var request = CreateValidRequest();
            var cancellationToken = new CancellationToken();
            var sut = await CreateSut();

            var currentUser = await (new CurrentUserServiceMock()).CurrentUser();

            var result = await sut.Handle(request, cancellationToken);

            Assert.Equal(roomName, result.TwilioRoomName);
            Assert.Equal(customerToken, result.TwilioVideoGrantForCustomerToken);
            Assert.Equal(technicianToken, result.TwilioVideoGrantForTechnicianToken);

            var transaction = ServiceProvider.GetService<DomainModelTransaction>();

            var supportTicket = transaction.Entities<SupportTicket>().Where(p => p.SupportTicketId == new Guid(request.SupportTicketId)).SingleOrDefault();
            Assert.Equal(roomName, supportTicket.TwilioRoomName);
            Assert.Equal(roomSid, supportTicket.TwilioRoomSid);
            Assert.Equal(customerToken, supportTicket.TwilioVideoGrantForCustomerToken);
            Assert.Equal(technicianToken, supportTicket.TwilioVideoGrantForTechnicianToken);
            Assert.Equal(CurrentDateTimeServiceMock.MockedDateTime(), supportTicket.CallStartedAt);
            Assert.Equal(currentUser.Id, supportTicket.TechnicianUserId);
        }

        [Fact]
        public async void Given_RequestIsValid_When_CreateRoomHandlerIsCalled_Then_AMessageIsSentToTheCustomer()
        {
            var request = CreateValidRequest();
            var cancellationToken = new CancellationToken();
            var sut = await CreateSut();

            var result = await sut.Handle(request, cancellationToken);

            Assert.Equal(1, customerHub.TechnicianHasStartedCallCounter);
            Assert.Equal(request.SupportTicketId, customerHub.LatestStartedCallMessage.SupportTicketId);
        }
        [Fact]
        public async void Given_RequestIsValid_When_CreateRoomHandlerIsCalled_Then_AMessageIsSentToTheTechnicians()
        {
            var request = CreateValidRequest();
            var cancellationToken = new CancellationToken();
            var sut = await CreateSut();

            var result = await sut.Handle(request, cancellationToken);
            var supportTicket = technicianHub.LatestSupportTicketUpdatedMessage.SupportTicket;

            Assert.Equal(1, technicianHub.SupportTicketUpdatedMessageCounter);
            Assert.Equal(request.SupportTicketId.ToLower(), supportTicket.SupportTicketId.ToString());
            Assert.Equal("Donald Duck", supportTicket.TechnicianFullName);
        }

        [Fact]
        public async void Given_SupportTicketIsNotInTheDatabase_When_CreateRoomHandlerIsCalled_Then_AnExceptionIsThrown()
        {
            var request = CreateValidRequest();

            request.SupportTicketId = "E9BC048E-10C7-4A9E-AC28-6B75CA401207";

            var cancellationToken = new CancellationToken();
            var sut = await CreateSut();

            await Assert.ThrowsAsync<SupportTicketNotFoundException>(() => sut.Handle(request, cancellationToken));
        }
    }
}
