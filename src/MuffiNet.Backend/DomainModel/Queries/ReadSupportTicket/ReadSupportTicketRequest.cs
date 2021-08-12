using MediatR;

namespace MuffiNet.FrontendReact.DomainModel.Queries.ReadSupportTicket
{
    public class ReadSupportTicketRequest : IRequest<ReadSupportTicketResponse>
    {
        public bool IncludeCompletedSupportTickets { get; set; }
    }
}