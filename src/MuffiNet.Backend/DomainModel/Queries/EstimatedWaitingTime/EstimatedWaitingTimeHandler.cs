using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using MuffiNet.Backend.Exceptions;
using MuffiNet.Backend.Models;

namespace MuffiNet.Backend.DomainModel.Queries.EstimatedWaitingTime
{
    public class EstimatedWaitingTimeHandler : IRequestHandler<EstimatedWaitingTimeRequest, EstimatedWaitingTimeResponse>
    {
        private DomainModelTransaction domainModelTransaction;

        public const int CallDurationInMinutes = 8;

        public EstimatedWaitingTimeHandler(DomainModelTransaction domainModelTransaction)
        {
            this.domainModelTransaction = domainModelTransaction ?? throw new ArgumentNullException(nameof(domainModelTransaction));
        }

        public async Task<EstimatedWaitingTimeResponse> Handle(EstimatedWaitingTimeRequest request, CancellationToken cancellationToken)
        {
            var supportTicket = await domainModelTransaction.EntitiesNoTracking<SupportTicket>().WithSupportTicketId(request.SupportTicketId).SingleOrDefaultAsync();

            if (supportTicket is null)
                throw new SupportTicketNotFoundException(request.SupportTicketId);

            var q = domainModelTransaction.EntitiesNoTracking<SupportTicket>()
                .AddedToQueueBeforeThis(supportTicket.CreatedAt)
                .NotStarted();

            int numberOfSupportTicketsInQueueBeforeThisOne = await q.CountAsync();

            return new EstimatedWaitingTimeResponse()
            {
                EstimatedCallDurationInMinutes = CallDurationInMinutes,
                NumberOfUnansweredCalls = numberOfSupportTicketsInQueueBeforeThisOne,
                EstimatedWaitingTimeInMinutes = CallDurationInMinutes * numberOfSupportTicketsInQueueBeforeThisOne,
            };
        }
    }
}