using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
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
        private readonly IExampleService exampleService;
        private readonly ICurrentUserService currentUserService;

        public RequestOssIdFromOssHandler(DomainModelTransaction transaction, IExampleService exampleService, ICurrentUserService currentUserService)
        {
            this.transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
            this.exampleService = exampleService ?? throw new ArgumentNullException(nameof(exampleService));
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

            var ossId = await exampleService.PerformService(request.SupportTicketId, supportTicket.CustomerName, technicianInitials);

            // Save in Database
            supportTicket.OssId = ossId;
            await transaction.SaveChangesAsync();

            return new RequestOssIdFromOssResponse() { OssId = ossId };
        }
    }
}