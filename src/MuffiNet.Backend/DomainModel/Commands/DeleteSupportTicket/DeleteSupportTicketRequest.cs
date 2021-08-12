using MediatR;
using System;

namespace MuffiNet.FrontendReact.DomainModel.Commands.DeleteSupportTicket
{
    public class DeleteSupportTicketRequest : IRequest<DeleteSupportTicketResponse>
    {
        public Guid SupportTicketId { get; set; }
    }
}