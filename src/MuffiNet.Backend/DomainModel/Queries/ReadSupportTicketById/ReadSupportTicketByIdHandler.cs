using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MuffiNet.Backend.Models;
using static MuffiNet.Backend.DomainModel.Queries.ReadSupportTicketById.ReadSupportTicketByIdResponse;

namespace MuffiNet.Backend.DomainModel.Queries.ReadSupportTicketById
{
    public class ReadSupportTicketByIdHandler : IRequestHandler<ReadSupportTicketByIdRequest, ReadSupportTicketByIdResponse>
    {
        private readonly DomainModelTransaction domainModelTransaction;

        public ReadSupportTicketByIdHandler(DomainModelTransaction domainModelTransaction)
        {
            this.domainModelTransaction = domainModelTransaction ?? throw new ArgumentNullException(nameof(domainModelTransaction));
        }

        public async Task<ReadSupportTicketByIdResponse> Handle(ReadSupportTicketByIdRequest request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));
            if (request.SupportTicketId == Guid.Empty)
                throw new ArgumentException(nameof(request.SupportTicketId));

            var supportTickets = domainModelTransaction.EntitiesNoTracking<SupportTicket>();

            var query = from supportTicket in supportTickets.WithSupportTicketId(request.SupportTicketId)
                        select new SupportTicketRecord(
                            supportTicket.CustomerName,
                            supportTicket.CustomerEmail,
                            supportTicket.CustomerPhone,
                            supportTicket.SupportTicketId,
                            DateTime.SpecifyKind(supportTicket.CreatedAt, DateTimeKind.Utc),
                            supportTicket.Brand,
                            supportTicket.TwilioVideoGrantForCustomerToken,
                            supportTicket.TwilioVideoGrantForTechnicianToken,
                            supportTicket.OssId
                        );

            return new ReadSupportTicketByIdResponse()
            {
                SupportTicket = await query.FirstOrDefaultAsync<SupportTicketRecord>()
            };
        }
    }
}