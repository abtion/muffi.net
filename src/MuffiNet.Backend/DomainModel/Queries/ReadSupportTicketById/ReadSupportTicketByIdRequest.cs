using MediatR;
using System;

namespace MuffiNet.FrontendReact.DomainModel.Queries.ReadSupportTicketById
{
    public class ReadSupportTicketByIdRequest : IRequest<ReadSupportTicketByIdResponse>
    {
        public Guid SupportTicketId { get; set; }
    }
}
