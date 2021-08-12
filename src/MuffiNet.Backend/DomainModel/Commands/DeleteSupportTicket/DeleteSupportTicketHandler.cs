using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MuffiNet.Backend.Models;

namespace MuffiNet.Backend.DomainModel.Commands.DeleteSupportTicket
{
    public class DeleteSupportTicketHandler : IRequestHandler<DeleteSupportTicketRequest, DeleteSupportTicketResponse>
    {
        private readonly DomainModelTransaction domainModelTransaction;

        public DeleteSupportTicketHandler(DomainModelTransaction domainModelTransaction)
        {
            this.domainModelTransaction = domainModelTransaction ?? throw new ArgumentNullException(nameof(domainModelTransaction));
        }

        public async Task<DeleteSupportTicketResponse> Handle(DeleteSupportTicketRequest request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));
            if (request.SupportTicketId == Guid.Empty)
                throw new ArgumentException(nameof(request.SupportTicketId));

            domainModelTransaction.Remove(
              domainModelTransaction.Entities<SupportTicket>()
              .Single(supportTicket => supportTicket.SupportTicketId == request.SupportTicketId));
            await domainModelTransaction.SaveChangesAsync();
            return new DeleteSupportTicketResponse();
        }
    }
}
