using MediatR;
using System;

namespace WebAppReact.DomainModel.Queries.ReadSupportTicketById
{
    public class ReadSupportTicketByIdRequest : IRequest<ReadSupportTicketByIdResponse>
    {
        public Guid SupportTicketId { get; set; }
    }
}
