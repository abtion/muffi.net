using MediatR;

namespace WebAppReact.DomainModel.Commands.CompleteRoom
{
    public class CompleteRoomRequest : IRequest<CompleteRoomResponse>
    {
        public string SupportTicketId { get; set; }
    }
}