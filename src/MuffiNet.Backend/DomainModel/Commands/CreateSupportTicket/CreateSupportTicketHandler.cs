using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using MuffiNet.Backend.Models;
using MuffiNet.Backend.Services;
using MuffiNet.Backend.HubContracts;

namespace MuffiNet.Backend.DomainModel.Commands.CreateSupportTicket
{
    public class CreateSupportTicketHandler : IRequestHandler<CreateSupportTicketRequest, CreateSupportTicketResponse>
    {
        private readonly DomainModelTransaction domainModelTransaction;
        private readonly ICurrentDateTimeService currentDateTimeService;
        private readonly IExampleHubContract exampleHub;

        public CreateSupportTicketHandler(DomainModelTransaction domainModelTransaction, ICurrentDateTimeService currentDateTimeService, IExampleHubContract exampleHub)
        {
            this.domainModelTransaction = domainModelTransaction ?? throw new ArgumentNullException(nameof(domainModelTransaction));
            this.currentDateTimeService = currentDateTimeService ?? throw new ArgumentNullException(nameof(currentDateTimeService));
            this.exampleHub = exampleHub ?? throw new ArgumentNullException(nameof(exampleHub));
        }

        public async Task<CreateSupportTicketResponse> Handle(CreateSupportTicketRequest request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrWhiteSpace(request.CustomerName))
                throw new ArgumentNullException(nameof(request.CustomerName));
            if (string.IsNullOrWhiteSpace(request.CustomerEmail))
                throw new ArgumentNullException(nameof(request.CustomerEmail));
            if (string.IsNullOrWhiteSpace(request.CustomerPhone))
                throw new ArgumentNullException(nameof(request.CustomerPhone));
            if (string.IsNullOrWhiteSpace(request.Brand))
                throw new ArgumentNullException(nameof(request.Brand));

            var supportTicket = new SupportTicket()
            {
                CustomerName = request.CustomerName,
                CustomerEmail = request.CustomerEmail,
                CustomerPhone = request.CustomerPhone,
                SupportTicketId = Guid.NewGuid(),
                Brand = request.Brand,
                CreatedAt = currentDateTimeService.CurrentDateTime(),
            };

            await domainModelTransaction.AddAsync<SupportTicket>(supportTicket);
            await domainModelTransaction.SaveChangesAsync();

            await exampleHub.SomeEntityCreated(new SomeEntityCreatedMessage(
                    supportTicket.SupportTicketId.ToString()
                )
            );

            return new CreateSupportTicketResponse() { SupportTicketId = supportTicket.SupportTicketId };
        }
    }
}