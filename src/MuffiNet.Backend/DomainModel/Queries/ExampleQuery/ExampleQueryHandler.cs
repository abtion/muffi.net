using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using MuffiNet.Backend.Exceptions;
using MuffiNet.Backend.Models;

namespace MuffiNet.Backend.DomainModel.Queries.ExampleQuery
{
    public class ExampleQueryHandler : IRequestHandler<ExampleQueryRequest, ExampleQueryResponse>
    {
        //private DomainModelTransaction domainModelTransaction;

        //public const int CallDurationInMinutes = 8;

        //public ExampleQueryHandler(DomainModelTransaction domainModelTransaction)
        //{
        //    this.domainModelTransaction = domainModelTransaction ?? throw new ArgumentNullException(nameof(domainModelTransaction));
        //}

        //public async Task<ExampleQueryResponse> Handle(ExampleQueryRequest request, CancellationToken cancellationToken)
        //{
        //    var supportTicket = await domainModelTransaction.EntitiesNoTracking<SupportTicket>().WithSupportTicketId(request.SupportTicketId).SingleOrDefaultAsync();

        //    if (supportTicket is null)
        //        throw new SupportTicketNotFoundException(request.SupportTicketId);

        //    var q = domainModelTransaction.EntitiesNoTracking<SupportTicket>()
        //        .AddedToQueueBeforeThis(supportTicket.CreatedAt)
        //        .NotStarted();

        //    int numberOfSupportTicketsInQueueBeforeThisOne = await q.CountAsync();

        //    return new ExampleQueryResponse()
        //    {
        //        EstimatedCallDurationInMinutes = CallDurationInMinutes,
        //        NumberOfUnansweredCalls = numberOfSupportTicketsInQueueBeforeThisOne,
        //        EstimatedWaitingTimeInMinutes = CallDurationInMinutes * numberOfSupportTicketsInQueueBeforeThisOne,
        //    };
        //}
        public Task<ExampleQueryResponse> Handle(ExampleQueryRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}