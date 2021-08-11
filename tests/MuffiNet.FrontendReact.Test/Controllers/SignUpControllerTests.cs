using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using MuffiNet.FrontendReact.Controllers;
using MuffiNet.FrontendReact.DomainModel;
using MuffiNet.FrontendReact.DomainModel.Commands.CreateSupportTicket;
using MuffiNet.FrontendReact.DomainModel.Queries.EstimatedWaitingTime;
using MuffiNet.FrontendReact.DomainModel.Queries.ReadVideoGrantForCustomerToken;
using MuffiNet.FrontendReact.Models;
using MuffiNet.FrontendReact.Services;
using Xunit;
using MuffiNet.Test.Shared.Mocks;

namespace MuffiNet.FrontendReact.Test.Controllers
{
    public class SignUpControllerTests : ControllerTest
    {
        [Fact]
        public async Task Given_RequestIsValid_When_CreateSupportTicketIsCalled_Then_ReturnTypeIsCorrect()
        {
            // Arrange
            var transaction = ServiceProvider.GetService<DomainModelTransaction>();
            var currentDateTimeMock = new Mock<ICurrentDateTimeService>();
            currentDateTimeMock.Setup(p => p.CurrentDateTime()).Returns(new DateTime(2021, 06, 17, 12, 05, 10));

            var technicianHubMock = new TechnicianHubMock();

            var controller = new SignUpController();
            var handler = new CreateSupportTicketHandler(transaction, currentDateTimeMock.Object, technicianHubMock);
            var request = new CreateSupportTicketRequest()
            {
                CustomerEmail = "a@b.c",
                CustomerName = "Donald Duck",
                CustomerPhone = "31331313",
                Brand = "apple"
            };

            var cancellationToken = new CancellationToken();

            // Act
            var response = await controller.CreateSupportTicket(handler, request, cancellationToken);

            // Assert
            Assert.IsType<CreateSupportTicketResponse>(response.Value);
        }

        [Fact]
        public async Task Given_RequestIsValid_When_ReadCustomerRoomGrantTokenIsCalled_Then_ReturnTypeIsCorrect()
        {
            // Arrange
            var transaction = ServiceProvider.GetService<DomainModelTransaction>();
            var userManager = ServiceProvider.GetService<UserManager<ApplicationUser>>();
            var supportTicketId = "5A444FE9-EF79-4F28-95EE-E472B482F56B";

            await TestData.CreateSupportTicketWithTwilioRoom(new Guid(supportTicketId));

            var controller = new SignUpController();
            var handler = new ReadVideoGrantForCustomerTokenHandler(transaction, userManager);

            var cancellationToken = new CancellationToken();

            // Act
            var response = await controller.ReadCustomerRoomGrantToken(handler, supportTicketId, cancellationToken);

            // Assert
            Assert.IsType<ReadVideoGrantForCustomerTokenResponse>(response.Value);
        }

        [Fact]
        public async Task Given_RequestIsValid_When_EstimatedWaitingTimeCalled_Then_ReturnTypeIsCorrect()
        {
            // Arrange
            var transaction = ServiceProvider.GetService<DomainModelTransaction>();
            var supportTicketId = "5A444FE9-EF79-4F28-95EE-E472B482F56B";

            await TestData.CreateSupportTicketWithInitalFields(supportTicketId, new DateTime(2021, 07, 01));

            var controller = new SignUpController();
            var handler = new EstimatedWaitingTimeHandler(transaction);

            var cancellationToken = new CancellationToken();

            // Act
            var response = await controller.EstimatedWaitingTime(handler, supportTicketId, cancellationToken);

            // Assert
            Assert.IsType<EstimatedWaitingTimeResponse>(response.Value);
        }
    }
}