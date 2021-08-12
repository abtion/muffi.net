using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using MuffiNet.FrontendReact.DomainModel;
using MuffiNet.FrontendReact.DomainModel.Queries.ReadVideoGrantForCustomerToken;
using MuffiNet.FrontendReact.Exceptions;
using MuffiNet.FrontendReact.Models;
using MuffiNet.Test.Shared.TestData;
using Xunit;

namespace MuffiNet.Backend.Tests.DomainModel.Queries.ReadSupportTicketById
{
    public class ReadVideoGrantForCustomerTokenHandlerTests : DomainModelTest<ReadVideoGrantForCustomerTokenHandler>
    {
        protected internal async override Task<ReadVideoGrantForCustomerTokenHandler> CreateSut()
        {
            var transaction = ServiceProvider.GetService<DomainModelTransaction>();
            var userManager = ServiceProvider.GetService<UserManager<ApplicationUser>>();

            return await Task.FromResult(new ReadVideoGrantForCustomerTokenHandler(transaction, userManager));
        }

        [Fact]
        public async void Given_SupportTicketIdIsValid_When_ReadVideoGrantForCustomerTokenHandlerIsCalled_Then_TheSupportTicketIsReturned()
        {
            var cancellationToken = new CancellationToken();
            var sut = await CreateSut();

            var supportTicketId = Guid.NewGuid();
            var supportTicket = await TestData.CreateSupportTicketWithTwilioRoom(supportTicketId);

            var applicationUser = ApplicationUserTestData.CreateApplicationUser();

            var request = new ReadVideoGrantForCustomerTokenRequest()
            {
                SupportTicketId = supportTicketId
            };

            var response = await sut.Handle(request, cancellationToken);

            Assert.Equal(supportTicket.CustomerName, response.Token.CustomerName);
            Assert.Equal(supportTicket.TwilioRoomName, response.Token.TwilioRoomName);
            Assert.Equal(supportTicket.TwilioVideoGrantForCustomerToken, response.Token.TwilioVideoGrantForCustomerToken);
            Assert.Equal("Donald Duck", response.Token.TechnicianFullName);
        }

        [Fact]
        public async void Given_SupportTicketWithRequestedIdIsNotInDB_When_ReadVideoGrantForCustomerTokenHandlerIsCalled_Then_SupportTicketNotFoundExceptionIsThrown()
        {
            var cancellationToken = new CancellationToken();
            var sut = await CreateSut();

            var supportTicket = await TestData.CreateSupportTicketWithTwilioRoom(Guid.NewGuid());

            var supportTicketId = Guid.NewGuid();
            var request = new ReadVideoGrantForCustomerTokenRequest()
            {
                SupportTicketId = supportTicketId
            };

            await Assert.ThrowsAsync<SupportTicketNotFoundException>(() => sut.Handle(request, cancellationToken));
        }

        [Fact]
        public async void Given_RequestIsNull_When_ReadVideoGrantForCustomerTokenHandlerIsCalled_Then_ThrowAnArgumentNullException()
        {
            var cancellationToken = new CancellationToken();
            var sut = await CreateSut();

            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Handle(null, cancellationToken));
        }

        [Fact]
        public async void Given_SupportTicketIdIsEmptyGuid_When_ReadVideoGrantForCustomerTokenHandlerIsCalled_Then_AnArgumentExceptionIsThrown()
        {
            var cancellationToken = new CancellationToken();
            var sut = await CreateSut();

            var request = new ReadVideoGrantForCustomerTokenRequest()
            {
                SupportTicketId = Guid.Empty
            };

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => sut.Handle(request, cancellationToken));
        }
    }
}