using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MuffiNet.Backend.DomainModel;
using MuffiNet.Backend.DomainModel.Queries.ReadSupportTicket;
using MuffiNet.Backend.Models;
using Xunit;

namespace MuffiNet.Backend.Tests.DomainModel.Queries.ReadSupportTicket
{
    public class ReadSupportTicketHandlerTests : DomainModelTest<ReadSupportTicketHandler>
    {
        protected internal async override Task<ReadSupportTicketHandler> CreateSut()
        {
            var transaction = ServiceProvider.GetService<DomainModelTransaction>();
            var userManager = ServiceProvider.GetService<UserManager<ApplicationUser>>();

            return await Task.FromResult(new ReadSupportTicketHandler(transaction, userManager));
        }

        private ReadSupportTicketRequest CreateValidRequest()
        {
            var request = new ReadSupportTicketRequest()
            {
                IncludeCompletedSupportTickets = false,
            };

            return request;
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(40)]
        public async void Given_XAmountOfSupportTicketsInTheDataBase_When_ReadSupportTicketHandlerIsCalled_Then_XAmountOfSupportTicketsAreReturned(int numberOfSupportTicketsToCreate)
        {
            var request = CreateValidRequest();
            var cancellationToken = new CancellationToken();
            var sut = await CreateSut();

            await TestData.CreateDemoSupportTicketsWhereCallHasStarted(numberOfSupportTicketsToCreate);

            var response = await sut.Handle(request, cancellationToken);

            Assert.Equal(numberOfSupportTicketsToCreate, response.SupportTickets.Count);
        }

        [Fact]
        public async void Given_SupportTicketIdIsValid_When_ReadSupportTicketHandlerIsCalled_Then_TheSupportTicketIsReturned()
        {
            var request = CreateValidRequest();
            var cancellationToken = new CancellationToken();
            var sut = await CreateSut();

            await TestData.CreateDemoSupportTicketsWhereCallHasStarted(1);

            var response = await sut.Handle(request, cancellationToken);

            var domainModelTransaction = ServiceProvider.GetService<DomainModelTransaction>();
            var supportTicket = domainModelTransaction.Entities<SupportTicket>().Single();

            Assert.Equal(supportTicket.Brand, response.SupportTickets.First().Brand);
            Assert.Equal(supportTicket.CallEndedAt, response.SupportTickets.First().CallEndedAt);
            Assert.Equal(supportTicket.CallStartedAt, response.SupportTickets.First().CallStartedAt);
            Assert.Equal(supportTicket.CreatedAt, response.SupportTickets.First().CreatedAt);
            Assert.Equal(supportTicket.CustomerName, response.SupportTickets.First().CustomerName);
            Assert.Equal(supportTicket.CustomerEmail, response.SupportTickets.First().CustomerEmail);
            Assert.Equal(supportTicket.CustomerPhone, response.SupportTickets.First().CustomerPhone);
            Assert.Equal(supportTicket.SupportTicketId, response.SupportTickets.First().SupportTicketId);
            Assert.Equal(supportTicket.TechnicianUserId, response.SupportTickets.First().TechnicianUserId);
            Assert.Equal("Donald Duck", response.SupportTickets.First().TechnicianFullName);
        }


        [Fact]
        public async void Given_IncludeCompletedeSupportTicketsIsTrue_When_ReadSupportTicketHandlerIsCalled_Then_CompletedSupportTicketAreReturned()
        {
            var request = CreateValidRequest();
            request.IncludeCompletedSupportTickets = true;

            var cancellationToken = new CancellationToken();
            var sut = await CreateSut();

            await TestData.CreateDemoSupportTicketsWhereCallHasStarted(5);
            await TestData.CreateDemoSupportTicketsWhereCallHasEnded(5);

            var response = await sut.Handle(request, cancellationToken);

            Assert.Equal(10, response.SupportTickets.Count);
        }

        [Fact]
        public async void Given_IncludeCompletedeSupportTicketsIsFalse_When_ReadSupportTicketHandlerIsCalled_Then_CompletedSupportTicketAreNotReturned()
        {
            var request = CreateValidRequest();
            request.IncludeCompletedSupportTickets = false;

            var cancellationToken = new CancellationToken();
            var sut = await CreateSut();

            await TestData.CreateDemoSupportTicketsWhereCallHasStarted(5);
            await TestData.CreateDemoSupportTicketsWhereCallHasEnded(5);

            var response = await sut.Handle(request, cancellationToken);

            Assert.Equal(5, response.SupportTickets.Count);
        }
    }
}