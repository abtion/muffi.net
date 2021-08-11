using MediatR;
using System;

namespace WebAppReact.DomainModel.Queries.EstimatedWaitingTime
{
    public class EstimatedWaitingTimeRequest : IRequest<EstimatedWaitingTimeResponse>
    {
        public Guid SupportTicketId { get; set; }
    }
}