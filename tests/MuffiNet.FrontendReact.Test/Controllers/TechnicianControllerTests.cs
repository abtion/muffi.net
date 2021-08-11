using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using MuffiNet.FrontendReact.Controllers;
using MuffiNet.FrontendReact.DomainModel;
using MuffiNet.FrontendReact.DomainModel.Commands.CompleteRoom;
using MuffiNet.FrontendReact.DomainModel.Commands.CreateRoom;
using MuffiNet.FrontendReact.DomainModel.Commands.CreateSupportTicket;
using MuffiNet.FrontendReact.DomainModel.Commands.DeleteSupportTicket;
using MuffiNet.FrontendReact.DomainModel.Commands.RequestOssIdFromOss;
using MuffiNet.FrontendReact.DomainModel.Queries.ReadSupportTicket;
using MuffiNet.FrontendReact.DomainModel.Queries.ReadSupportTicketById;
using MuffiNet.FrontendReact.Models;
using MuffiNet.FrontendReact.Services;
using MuffiNet.FrontendReact.Test.Mocks;
using MuffiNet.FrontendReact.Test.TestData;
using Xunit;

namespace MuffiNet.FrontendReact.Test.Controllers
{
    public class TechnicianControllerTests : ControllerTest
    {
        private static ICurrentDateTimeService MockCurrentDateTimeService()
        {
            var currentDateTimeMock = new Mock<ICurrentDateTimeService>();
            currentDateTimeMock.Setup(p => p.CurrentDateTime()).Returns(new DateTime(2021, 06, 17, 12, 05, 10));

            return currentDateTimeMock.Object;
        }
        private CustomerHubMock customerHub = new CustomerHubMock();

        [Fact]
        public async Task Given_RequestIsValid_When_ReadSupportTicketsIsCalled_Then_ReturnTypeIsCorrect()
        {
            // Arrange
            var transaction = ServiceProvider.GetService<DomainModelTransaction>();
            var userManager = ServiceProvider.GetService<UserManager<ApplicationUser>>();

            var controller = new AuthorizedController();
            var handler = new ReadSupportTicketHandler(transaction, userManager);

            var cancellationToken = new CancellationToken();

            // Act
            var response = await controller.ReadSupportTickets(handler, cancellationToken);

            // Assert
            Assert.IsType<ReadSupportTicketResponse>(response.Value);
        }

        [Fact]
        public async Task Given_RequestIsValid_When_DeleteSupportTicketIsCalled_Then_ReturnTypeIsCorrect()
        {
            // Arrange
            var transaction = ServiceProvider.GetService<DomainModelTransaction>();

            var technicianHubMock = new TechnicianHubMock();

            var createSupportTicketHandler = new CreateSupportTicketHandler(transaction, MockCurrentDateTimeService(), technicianHubMock);
            var createSupportTicketRequest = new CreateSupportTicketRequest()
            {
                CustomerEmail = "a@b.c",
                CustomerName = "Donald Duck",
                CustomerPhone = "31331313",
                Brand = "apple",
            };

            var createSupportTicketResponse = await createSupportTicketHandler.Handle(createSupportTicketRequest, new CancellationToken());

            var controller = new AuthorizedController();
            var handler = new DeleteSupportTicketHandler(transaction);
            var request = new DeleteSupportTicketRequest()
            {
                SupportTicketId = createSupportTicketResponse.SupportTicketId
            };

            var cancellationToken = new CancellationToken();

            // Act
            var response = await controller.DeleteSupportTicket(handler, request, cancellationToken);

            // Assert
            Assert.IsType<DeleteSupportTicketResponse>(response.Value);
        }

        [Fact]
        public async Task Given_RequestIsValid_When_CreateRoomTokenCalled_Then_ReturnTypeIsCorrect()
        {
            // Arrange
            var transaction = ServiceProvider.GetService<DomainModelTransaction>();

            var technicianHubMock = new TechnicianHubMock();

            var createSupportTicketHandler = new CreateSupportTicketHandler(transaction, MockCurrentDateTimeService(), technicianHubMock);
            var createSupportTicketRequest = new CreateSupportTicketRequest()
            {
                CustomerEmail = "a@b.c",
                CustomerName = "Donald Duck",
                CustomerPhone = "31331313",
                Brand = "apple",
            };

            var createSupportTicketResponse = await createSupportTicketHandler.Handle(createSupportTicketRequest, new CancellationToken());

            var controller = new AuthorizedController();

            var roomName = "TheGreenRoom";
            var roomSid = "sid";

            var mock = new Mock<ITwilioService>();
            mock.Setup(p => p.CreateRoom(roomName)).ReturnsAsync(roomSid);
            mock.Setup(p => p.CreateVideoGrant("technician", roomName)).Returns(Guid.NewGuid().ToString());
            mock.Setup(p => p.CreateVideoGrant("customer", roomName)).Returns(Guid.NewGuid().ToString());

            ITwilioService mockedVideoService = mock.Object;
            var customerHubMock = new CustomerHubMock();
            var currentUserServiceMock = new CurrentUserServiceMock();
            var userManager = ServiceProvider.GetService<UserManager<ApplicationUser>>();

            var handler = new CreateRoomHandler(mockedVideoService, transaction, customerHubMock, technicianHubMock, CurrentDateTimeServiceMock.MockCurrentDateTimeService(), currentUserServiceMock, userManager);
            var request = new CreateRoomRequest()
            {
                SupportTicketId = createSupportTicketResponse.SupportTicketId.ToString(),
            };

            var cancellationToken = new CancellationToken();

            // Act
            var response = await controller.CreateRoomToken(handler, request, cancellationToken);

            // Assert
            Assert.IsType<CreateRoomResponse>(response.Value);
        }

        [Fact]
        public async Task Given_RequestIsValid_When_ReadSupportTicketByIdIsCalled_Then_ReturnTypeIsCorrect()
        {
            // Arrange
            var transaction = ServiceProvider.GetService<DomainModelTransaction>();

            var controller = new AuthorizedController();
            var handler = new ReadSupportTicketByIdHandler(transaction);
            var supportTicketId = Guid.NewGuid().ToString();

            var cancellationToken = new CancellationToken();

            // Act
            var response = await controller.ReadSupportTicketById(handler, supportTicketId, cancellationToken);

            // Assert
            Assert.IsType<ReadSupportTicketByIdResponse>(response.Value);
        }

        [Fact]
        public async Task Given_RequestIsValid_When_CompleteRoomIsCalled_Then_ReturnTypeIsCorrect()
        {
            // Arrange
            var transaction = ServiceProvider.GetService<DomainModelTransaction>();
            var supportTicketTestData = new SupportTicketTestData(transaction);
            var testSupportTicket = await supportTicketTestData.CreateDemoSupportTicketsWhereCallHasStarted();

            var technicianHubMock = new TechnicianHubMock();
            var customerHubMock = new CustomerHubMock();

            var mock = new Mock<ITwilioService>();
            mock.Setup(p => p.CompleteRoom(testSupportTicket.TwilioRoomName));

            ITwilioService mockedTwilioService = mock.Object;

            var controller = new AuthorizedController();
            var handler = new CompleteRoomHandler(transaction, CurrentDateTimeServiceMock.MockCurrentDateTimeService(), customerHubMock, technicianHubMock, mockedTwilioService);

            var request = new CompleteRoomRequest() { SupportTicketId = testSupportTicket.SupportTicketId.ToString() };

            var cancellationToken = new CancellationToken();

            // Act
            var response = await controller.CompleteRoom(handler, request, cancellationToken);

            // Assert
            Assert.IsType<CompleteRoomResponse>(response.Value);
        }

        [Fact]
        public async Task Given_RequestIsValid_When_RequestOssIdFromOssIsCalled_Then_ReturnTypeIsCorrect()
        {
            // Arrange
            var transaction = ServiceProvider.GetService<DomainModelTransaction>();

            var technicianHubMock = new TechnicianHubMock();
            var care1ServiceMock = new Care1ServiceMock();
            var currentUserServiceMock = new CurrentUserServiceMock();

            var createSupportTicketHandler = new CreateSupportTicketHandler(transaction, MockCurrentDateTimeService(), technicianHubMock);
            var createSupportTicketRequest = new CreateSupportTicketRequest()
            {
                CustomerEmail = "a@b.c",
                CustomerName = "Donald Duck",
                CustomerPhone = "31331313",
                Brand = "apple",
            };

            var createSupportTicketResponse = await createSupportTicketHandler.Handle(createSupportTicketRequest, new CancellationToken());

            var controller = new AuthorizedController();
            var handler = new RequestOssIdFromOssHandler(transaction, customerHub, care1ServiceMock, currentUserServiceMock);
            var supportTicketId = createSupportTicketResponse.SupportTicketId.ToString();

            var cancellationToken = new CancellationToken();

            // Act
            var response = await controller.RequestOssIdFromOss(handler, supportTicketId, cancellationToken);

            // Assert
            Assert.IsType<RequestOssIdFromOssResponse>(response.Value);
        }
    }
}
