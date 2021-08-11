using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using MuffiNet.FrontendReact.Controllers;
using MuffiNet.FrontendReact.DomainModel;
using MuffiNet.FrontendReact.DomainModel.Commands.CreateSupportTicket;
using MuffiNet.FrontendReact.DomainModel.Commands.DeleteSupportTicket;
using MuffiNet.FrontendReact.DomainModel.Commands.RequestOssIdFromOss;
using MuffiNet.FrontendReact.DomainModel.Queries.ReadSupportTicket;
using MuffiNet.FrontendReact.DomainModel.Queries.ReadSupportTicketById;
using MuffiNet.FrontendReact.Models;
using MuffiNet.FrontendReact.Services;
using MuffiNet.Test.Shared.Mocks;
using MuffiNet.Test.Shared.TestData;
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

            var exampleHubMock = new ExampleHubMock();

            var createSupportTicketHandler = new CreateSupportTicketHandler(transaction, MockCurrentDateTimeService(), exampleHubMock);
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
        public async Task Given_RequestIsValid_When_RequestOssIdFromOssIsCalled_Then_ReturnTypeIsCorrect()
        {
            // Arrange
            var transaction = ServiceProvider.GetService<DomainModelTransaction>();

            var exampleHubMock = new ExampleHubMock();
            var exampleServiceMock = new ExampleServiceMock();
            var currentUserServiceMock = new CurrentUserServiceMock();

            var createSupportTicketHandler = new CreateSupportTicketHandler(transaction, MockCurrentDateTimeService(), exampleHubMock);
            var createSupportTicketRequest = new CreateSupportTicketRequest()
            {
                CustomerEmail = "a@b.c",
                CustomerName = "Donald Duck",
                CustomerPhone = "31331313",
                Brand = "apple",
            };

            var createSupportTicketResponse = await createSupportTicketHandler.Handle(createSupportTicketRequest, new CancellationToken());

            var controller = new AuthorizedController();
            var handler = new RequestOssIdFromOssHandler(transaction, exampleServiceMock, currentUserServiceMock);
            var supportTicketId = createSupportTicketResponse.SupportTicketId.ToString();

            var cancellationToken = new CancellationToken();

            // Act
            var response = await controller.RequestOssIdFromOss(handler, supportTicketId, cancellationToken);

            // Assert
            Assert.IsType<RequestOssIdFromOssResponse>(response.Value);
        }
    }
}
