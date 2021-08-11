using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebAppReact.DomainModel;
using WebAppReact.DomainModel.Queries.ReadSupportTicketById;
using Xunit;

namespace WebAppReact.Test.DomainModel.Queries.ReadSupportTicketById
{
    public class ReadSupportTicketByIdHandlerTests : DomainModelTest<ReadSupportTicketByIdHandler>
    {
        protected internal override async Task<ReadSupportTicketByIdHandler> CreateSut()
        {
            return await Task.FromResult(new ReadSupportTicketByIdHandler(ServiceProvider.GetService<DomainModelTransaction>()));
        }

        [Fact]
        public async void Given_SupportTicketIdIsValid_When_ReadSuportTicketByIdHandlerIsCalled_Then_TheSupportTicketIsReturned()
        {
            var cancellationToken = new CancellationToken();
            var sut = await CreateSut();

            var supportTicketId = Guid.NewGuid();
            var supportTicket = await TestData.CreateSupportTicketWithTwilioRoom(supportTicketId);

            var request = new ReadSupportTicketByIdRequest()
            {
                SupportTicketId = supportTicketId
            };

            var response = await sut.Handle(request, cancellationToken);

            Assert.Equal(supportTicketId, response.SupportTicket.SupportTicketId);
            Assert.Equal(supportTicket.CustomerName, response.SupportTicket.CustomerName);
            Assert.Equal(supportTicket.CustomerEmail, response.SupportTicket.CustomerEmail);
            Assert.Equal(supportTicket.CustomerPhone, response.SupportTicket.CustomerPhone);
            Assert.Equal(supportTicket.CreatedAt, response.SupportTicket.CreatedAt);
            Assert.Equal(supportTicket.Brand, response.SupportTicket.Brand);
            Assert.Equal(supportTicket.TwilioVideoGrantForTechnicianToken, response.SupportTicket.TwilioVideoGrantForTechnicianToken);
            Assert.Equal(supportTicket.TwilioVideoGrantForCustomerToken, response.SupportTicket.TwilioVideoGrantForCustomerToken);
            Assert.Equal(supportTicket.OssId, response.SupportTicket.OssId);
        }

        [Fact]
        public async void Given_SupportTicketWithRequestedIdIsNotInDB_When_ReadSuportTicketByIdHandlerIsCalled_Then_SupportTicketIsNull()
        {
            var cancellationToken = new CancellationToken();
            var sut = await CreateSut();

            await TestData.CreateSupportTicketWithTwilioRoom(Guid.NewGuid());

            var supportTicketId = Guid.NewGuid();
            var request = new ReadSupportTicketByIdRequest()
            {
                SupportTicketId = supportTicketId
            };

            var response = await sut.Handle(request, cancellationToken);

            Assert.Null(response.SupportTicket);
        }

        [Fact]
        public async void Given_RequestIsNull_When_ReadSuportTicketByIdHandlerIsCalled_Then_ThrowAnArgumentNullException()
        {
            var cancellationToken = new CancellationToken();
            var sut = await CreateSut();

            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Handle(null, cancellationToken));
        }

        [Fact]
        public async void Given_SupportTicketIdIsEmptyGuid_When_ReadSuportTicketByIdHandlerIsCalled_Then_AnArgumentExceptionIsThrown()
        {
            var cancellationToken = new CancellationToken();
            var sut = await CreateSut();

            var request = new ReadSupportTicketByIdRequest()
            {
                SupportTicketId = Guid.Empty
            };

            await Assert.ThrowsAsync<ArgumentException>(() => sut.Handle(request, cancellationToken));
        }
    }
}
