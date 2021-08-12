using MediatR;
using System;

namespace MuffiNet.Backend.DomainModel.Queries.EstimatedWaitingTime
{
    public class EstimatedWaitingTimeRequest : IRequest<EstimatedWaitingTimeResponse>
    {
        public Guid SupportTicketId { get; set; }
    }
}