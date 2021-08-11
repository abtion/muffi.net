using MediatR;

namespace WebAppReact.DomainModel.Queries.ReadSupportTicket
{
    public class ReadSupportTicketRequest : IRequest<ReadSupportTicketResponse>
    {
        public bool IncludeCompletedSupportTickets { get; set; }
    }
}