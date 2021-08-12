using MediatR;
using System;

namespace MuffiNet.FrontendReact.DomainModel.Queries.EstimatedWaitingTime
{
    public class EstimatedWaitingTimeRequest : IRequest<EstimatedWaitingTimeResponse>
    {
        public Guid SupportTicketId { get; set; }
    }
}