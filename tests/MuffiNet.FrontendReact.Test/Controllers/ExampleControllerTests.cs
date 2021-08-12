using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using MuffiNet.FrontendReact.Controllers;
using MuffiNet.Backend.DomainModel;
using MuffiNet.Backend.Models;
using MuffiNet.Backend.Services;
using MuffiNet.Test.Shared.Mocks;
using MuffiNet.Test.Shared.TestData;
using Xunit;

namespace MuffiNet.FrontendReact.Test.Controllers
{
    public class ExampleControllerTests : ControllerTest
    {
        // MOCK datetime service
        
        // private static ICurrentDateTimeService MockCurrentDateTimeService()
        // {
        //     var currentDateTimeMock = new Mock<ICurrentDateTimeService>();
        //     currentDateTimeMock.Setup(p => p.CurrentDateTime()).Returns(new DateTime(2021, 06, 17, 12, 05, 10));

        //     return currentDateTimeMock.Object;
        // }

        [Fact]
        public async Task Given_RequestIsValid_When_ExampleQueryIsCalled_Then_ReturnTypeIsCorrect()
        {
            //// Arrange
            //var transaction = ServiceProvider.GetService<DomainModelTransaction>();
            //var userManager = ServiceProvider.GetService<UserManager<ApplicationUser>>();

            //var controller = new ExampleController();
            //var handler = new ExampleQueryHandler(transaction, userManager);

            //var cancellationToken = new CancellationToken();

            //// Act
            //var response = await controller.ReadSupportTickets(handler, cancellationToken);

            //// Assert
            //Assert.IsType<ExampleQueryResponse>(response.Value);
        }

        [Fact]
        public async Task Given_RequestIsValid_When_ExampleCommandIsCalled_Then_ReturnTypeIsCorrect()
        {
            //// Arrange
            //var transaction = ServiceProvider.GetService<DomainModelTransaction>();

            //var exampleHubMock = new ExampleHubMock();

            //var createSupportTicketHandler = new ExampleCommandHandler(transaction, MockCurrentDateTimeService(), exampleHubMock);
            //var createSupportTicketRequest = new ExampleCommandRequest()
            //{
            //    CustomerEmail = "a@b.c",
            //    CustomerName = "Donald Duck",
            //    CustomerPhone = "31331313",
            //    Brand = "apple",
            //};

            //var createSupportTicketResponse = await createSupportTicketHandler.Handle(createSupportTicketRequest, new CancellationToken());

            //var controller = new AuthorizedController();
            //var handler = new DeleteSupportTicketHandler(transaction);
            //var request = new DeleteSupportTicketRequest()
            //{
            //    SupportTicketId = createSupportTicketResponse.SupportTicketId
            //};

            //var cancellationToken = new CancellationToken();

            //// Act
            //var response = await controller.DeleteSupportTicket(handler, request, cancellationToken);

            //// Assert
            //Assert.IsType<DeleteSupportTicketResponse>(response.Value);
        }
    }
}