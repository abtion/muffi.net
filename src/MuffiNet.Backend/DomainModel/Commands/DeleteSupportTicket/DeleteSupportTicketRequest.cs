using MediatR;
using System;

namespace MuffiNet.Backend.DomainModel.Commands.DeleteSupportTicket
{
    public class DeleteSupportTicketRequest : IRequest<DeleteSupportTicketResponse>
    {
        public Guid SupportTicketId { get; set; }
    }
}