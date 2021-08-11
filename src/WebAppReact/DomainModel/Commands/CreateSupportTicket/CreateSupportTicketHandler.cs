using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebAppReact.Hubs;
using WebAppReact.Models;
using WebAppReact.Services;

namespace WebAppReact.DomainModel.Commands.CreateSupportTicket
{
    public class CreateSupportTicketHandler : IRequestHandler<CreateSupportTicketRequest, CreateSupportTicketResponse>
    {
        private readonly DomainModelTransaction domainModelTransaction;
        private readonly ICurrentDateTimeService currentDateTimeService;
        private readonly TechnicianHub technicianHub;

        public CreateSupportTicketHandler(DomainModelTransaction domainModelTransaction, ICurrentDateTimeService currentDateTimeService, TechnicianHub technicianHub)
        {
            this.domainModelTransaction = domainModelTransaction ?? throw new ArgumentNullException(nameof(domainModelTransaction));
            this.currentDateTimeService = currentDateTimeService ?? throw new ArgumentNullException(nameof(currentDateTimeService));
            this.technicianHub = technicianHub ?? throw new ArgumentNullException(nameof(technicianHub));
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

            await technicianHub.SupportTicketCreated(new SupportTicketCreatedMessage(
                new Queries.ReadSupportTicket.ReadSupportTicketResponse.SupportTicketRecord(
                    supportTicket.CustomerName,
                    supportTicket.CustomerEmail,
                    supportTicket.CustomerPhone,
                    supportTicket.SupportTicketId,
                    supportTicket.CreatedAt,
                    supportTicket.Brand,
                    null,
                    null,
                    "",
                    "")
                )
            );

            return new CreateSupportTicketResponse() { SupportTicketId = supportTicket.SupportTicketId };
        }
    }
}