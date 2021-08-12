using MediatR;
namespace MuffiNet.Backend.DomainModel.Commands.RequestOssIdFromOss
{
    public class RequestOssIdFromOssRequest : IRequest<RequestOssIdFromOssResponse>
    {
        public string SupportTicketId { get; set; }
    }
}