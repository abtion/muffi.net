using MediatR;

namespace MuffiNet.Backend.DomainModel.Queries.ReadSupportTicket
{
    public class ReadSupportTicketRequest : IRequest<ReadSupportTicketResponse>
    {
        public bool IncludeCompletedSupportTickets { get; set; }
    }
}