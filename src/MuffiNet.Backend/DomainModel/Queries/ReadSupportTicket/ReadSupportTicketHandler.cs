using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MuffiNet.Backend.Models;
using static MuffiNet.Backend.DomainModel.Queries.ReadSupportTicket.ReadSupportTicketResponse;

namespace MuffiNet.Backend.DomainModel.Queries.ReadSupportTicket
{
    public class ReadSupportTicketHandler : IRequestHandler<ReadSupportTicketRequest, ReadSupportTicketResponse>
    {
        private readonly DomainModelTransaction domainModelTransaction;
        private readonly UserManager<ApplicationUser> userManager;

        public ReadSupportTicketHandler(DomainModelTransaction domainModelTransaction, UserManager<ApplicationUser> userManager)
        {
            this.domainModelTransaction = domainModelTransaction ?? throw new ArgumentNullException(nameof(domainModelTransaction));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<ReadSupportTicketResponse> Handle(ReadSupportTicketRequest request, CancellationToken cancellationToken)
        {
            var supportTickets = domainModelTransaction.EntitiesNoTracking<SupportTicket>();

            if (!request.IncludeCompletedSupportTickets)
                supportTickets = supportTickets.NotCompleted();

            var userQuery = from user in userManager.Users
                            select user;

            var users = await userQuery.ToListAsync();

            var query =
                from supportTicket in supportTickets.OrderedByCreatedTime()
                select new SupportTicketRecord(
                    supportTicket.CustomerName,
                    supportTicket.CustomerEmail,
                    supportTicket.CustomerPhone,
                    supportTicket.SupportTicketId,
                    DateTime.SpecifyKind(supportTicket.CreatedAt, DateTimeKind.Utc),
                    supportTicket.Brand,
                    supportTicket.CallStartedAt.HasValue ? DateTime.SpecifyKind(supportTicket.CallStartedAt.Value, DateTimeKind.Utc) : null,
                    supportTicket.CallEndedAt.HasValue ? DateTime.SpecifyKind(supportTicket.CallEndedAt.Value, DateTimeKind.Utc) : null,
                    supportTicket.TechnicianUserId,
                    TechnicianUserName(supportTicket, users));

            return new ReadSupportTicketResponse() { SupportTickets = await query.ToListAsync(cancellationToken) };
        }

        private static string TechnicianUserName(SupportTicket supportTicket, List<ApplicationUser> users)
        {
            var user = users.Where(user => user.Id == supportTicket.TechnicianUserId).SingleOrDefault();

            if (user != null)
                return string.IsNullOrEmpty(user.FullName) ? "N/A" : user.FullName;

            return string.Empty;
        }
    }
}
