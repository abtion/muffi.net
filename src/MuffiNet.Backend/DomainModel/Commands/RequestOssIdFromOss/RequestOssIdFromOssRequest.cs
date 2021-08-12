using MediatR;
namespace MuffiNet.FrontendReact.DomainModel.Commands.RequestOssIdFromOss
{
    public class RequestOssIdFromOssRequest : IRequest<RequestOssIdFromOssResponse>
    {
        public string SupportTicketId { get; set; }
    }
}