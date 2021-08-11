using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MuffiNet.FrontendReact.DomainModel;
using MuffiNet.FrontendReact.DomainModel.Commands.DeleteSupportTicket;
using MuffiNet.FrontendReact.Models;
using Xunit;
namespace MuffiNet.FrontendReact.Test.DomainModel.Commands.DeleteSupportTicket
{
    public class DeleteSupportTicketHandlerTests : DomainModelTest<DeleteSupportTicketHandler>
    {
        protected internal async override Task<DeleteSupportTicketHandler> CreateSut()
        {
            return await Task.FromResult(new DeleteSupportTicketHandler(ServiceProvider.GetService<DomainModelTransaction>()));
        }

        private async Task CreateDemoSupportTicket(Guid supportTicketId)
        {
            var domainModelTransaction = ServiceProvider.GetService<DomainModelTransaction>();

            var supportTicket = new SupportTicket
            {
                CustomerName = "Ruth",
                CustomerEmail = "Ruth@msn.dk",
                CustomerPhone = "12345678",
                SupportTicketId = supportTicketId
            };

            await domainModelTransaction.AddAsync<SupportTicket>(supportTicket);
            await domainModelTransaction.SaveChangesAsync();
        }

        [Fact]
        public async void Given_RequestIsValid_When_DeleteSuportTicketHandlerIsCalled_Then_SupportTicketIsDeleted()
        {
            var cancellationToken = new CancellationToken();
            var sut = await CreateSut();

            var supportTicketId = Guid.NewGuid();
            await CreateDemoSupportTicket(supportTicketId);

            var request = new DeleteSupportTicketRequest()
            {
                SupportTicketId = supportTicketId
            };

            await sut.Handle(request, cancellationToken);

            Assert.Equal(0, ServiceProvider.GetService<DomainModelTransaction>().EntitiesNoTracking<SupportTicket>().Count());
        }

        [Fact]
        public async void Given_TwoSupportTicketsInTheDB_When_DeleteSuportTicketHandlerIsCalled_Then_TheCorrectSupportTicketIsDeleted()
        {
            var cancellationToken = new CancellationToken();
            var sut = await CreateSut();

            var supportTicketIdToBeDeleted = Guid.NewGuid();
            var supportTicketId = Guid.NewGuid();

            await CreateDemoSupportTicket(supportTicketIdToBeDeleted);
            await CreateDemoSupportTicket(supportTicketId);

            var request = new DeleteSupportTicketRequest()
            {
                SupportTicketId = supportTicketIdToBeDeleted
            };

            await sut.Handle(request, cancellationToken);

            Assert.Equal(1, ServiceProvider.GetService<DomainModelTransaction>().EntitiesNoTracking<SupportTicket>().Count());
            Assert.Equal(supportTicketId, ServiceProvider.GetService<DomainModelTransaction>().EntitiesNoTracking<SupportTicket>().First().SupportTicketId);
        }

        [Fact]
        public async void Given_RequestIsNull_When_DeleteSuportTicketHandlerIsCalled_Then_AnArgumentNullExceptionIsThrown()
        {
            var cancellationToken = new CancellationToken();
            var sut = await CreateSut();

            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Handle(null, cancellationToken));
        }

        [Fact]
        public async void Given_SupportTicketIdIsEmptyGuid_When_DeleteSuportTicketHandlerIsCalled_Then_AnArgumentExceptionIsThrown()
        {
            var cancellationToken = new CancellationToken();
            var sut = await CreateSut();

            var request = new DeleteSupportTicketRequest()
            {
                SupportTicketId = Guid.Empty
            };

            await Assert.ThrowsAsync<ArgumentException>(() => sut.Handle(request, cancellationToken));
        }
    }
}