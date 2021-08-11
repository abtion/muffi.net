using MediatR;
using System;

namespace WebAppReact.DomainModel.Commands.DeleteSupportTicket
{
    public class DeleteSupportTicketRequest : IRequest<DeleteSupportTicketResponse>
    {
        public Guid SupportTicketId { get; set; }
    }
}