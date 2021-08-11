using MediatR;
namespace WebAppReact.DomainModel.Commands.RequestOssIdFromOss {
    public class RequestOssIdFromOssRequest : IRequest<RequestOssIdFromOssResponse>
    {
        public string SupportTicketId { get; set; }
    }
}