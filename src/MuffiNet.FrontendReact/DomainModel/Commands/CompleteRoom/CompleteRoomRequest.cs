using MediatR;

namespace MuffiNet.FrontendReact.DomainModel.Commands.CompleteRoom
{
    public class CompleteRoomRequest : IRequest<CompleteRoomResponse>
    {
        public string SupportTicketId { get; set; }
    }
}