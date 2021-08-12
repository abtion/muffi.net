using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using MuffiNet.Backend.Exceptions;
using MuffiNet.Backend.Models;

namespace MuffiNet.Backend.DomainModel.Queries.ReadVideoGrantForCustomerToken
{
    public class ReadVideoGrantForCustomerTokenHandler : IRequestHandler<ReadVideoGrantForCustomerTokenRequest, ReadVideoGrantForCustomerTokenResponse>
    {
        private readonly DomainModelTransaction domainModelTransaction;
        private readonly UserManager<ApplicationUser> userManager;

        public ReadVideoGrantForCustomerTokenHandler(DomainModelTransaction domainModelTransaction, UserManager<ApplicationUser> userManager)
        {
            this.domainModelTransaction = domainModelTransaction ?? throw new ArgumentNullException(nameof(domainModelTransaction));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<ReadVideoGrantForCustomerTokenResponse> Handle(ReadVideoGrantForCustomerTokenRequest request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            if (request.SupportTicketId == Guid.Empty)
                throw new ArgumentOutOfRangeException(nameof(request.SupportTicketId));

            var supportTicket = await domainModelTransaction.EntitiesNoTracking<SupportTicket>().WithSupportTicketId(request.SupportTicketId).SingleOrDefaultAsync(cancellationToken);

            if (supportTicket is null)
                throw new SupportTicketNotFoundException(request.SupportTicketId);

            string technicianName = "Name not found";

            if (!string.IsNullOrWhiteSpace(supportTicket.TechnicianUserId))
            {
                var technician = await userManager.FindByIdAsync(supportTicket.TechnicianUserId);
                if (technician != null)
                    technicianName = "Hello world";
            }

            return new ReadVideoGrantForCustomerTokenResponse()
            {
                Token = new ReadVideoGrantForCustomerTokenResponse.VideoGrantForCustomerToken(
                    supportTicket.CustomerName,
                    supportTicket.TwilioRoomName,
                    supportTicket.TwilioVideoGrantForCustomerToken,
                    technicianName,
                    supportTicket.OssId)
            };
        }
    }
}