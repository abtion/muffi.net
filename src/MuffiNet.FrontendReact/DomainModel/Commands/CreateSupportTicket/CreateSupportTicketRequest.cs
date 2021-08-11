using MediatR;

namespace MuffiNet.FrontendReact.DomainModel.Commands.CreateSupportTicket
{
    public class CreateSupportTicketRequest : IRequest<CreateSupportTicketResponse>
    {
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string Brand { get; set; }
    }
}
