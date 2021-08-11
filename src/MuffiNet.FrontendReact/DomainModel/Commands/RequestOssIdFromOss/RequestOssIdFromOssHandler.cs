using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using MuffiNet.FrontendReact.Hubs;
using MuffiNet.FrontendReact.Models;
using MuffiNet.FrontendReact.Services;
using Microsoft.EntityFrameworkCore;
using MuffiNet.FrontendReact.Exceptions;
using System.Linq;

namespace MuffiNet.FrontendReact.DomainModel.Commands.RequestOssIdFromOss
{
    public class RequestOssIdFromOssHandler : IRequestHandler<RequestOssIdFromOssRequest, RequestOssIdFromOssResponse>
    {
        private readonly DomainModelTransaction transaction;
        private readonly CustomerHub customerHub;
        private readonly ICare1Service care1Service;
        private readonly ICurrentUserService currentUserService;

        public RequestOssIdFromOssHandler(DomainModelTransaction transaction, CustomerHub customerHub, ICare1Service care1Service, ICurrentUserService currentUserService)
        {
            this.transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
            this.customerHub = customerHub ?? throw new ArgumentNullException(nameof(customerHub));
            this.care1Service = care1Service ?? throw new ArgumentNullException(nameof(care1Service));
            this.currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        }

        public async Task<RequestOssIdFromOssResponse> Handle(RequestOssIdFromOssRequest request, CancellationToken cancellationToken)
        {
            // Get support ticket from id
            var supportTicketId = new Guid(request.SupportTicketId);

            var query = from st in transaction.Entities<SupportTicket>()
                        where st.SupportTicketId == supportTicketId
                        select st;

            if (!query.Any())
                throw new SupportTicketNotFoundException(supportTicketId);

            var supportTicket = await query.FirstOrDefaultAsync();

            // Get OssId From OSS
            var technicianInitials = (await currentUserService.CurrentUser()).Email.Split('@').FirstOrDefault();

            var ossId = await care1Service.CreateLinkToOss(request.SupportTicketId, supportTicket.CustomerName, technicianInitials);

            // Save in Database
            supportTicket.OssId = ossId;
            await transaction.SaveChangesAsync();

            // Notify customer
            await customerHub.TechnicianHasCreatedOssCase(new TechnicianHasCreatedOssCaseMessage(request.SupportTicketId, supportTicket.OssId));

            return new RequestOssIdFromOssResponse() { OssId = ossId };
        }
    }
}